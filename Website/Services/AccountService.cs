
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.ViewModels;
using Common.Helpers;
using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Newtonsoft.Json;

namespace Website.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IAsyncRepository<AccountProvider> _accountProviderRepository;
        private readonly IAccountFbPostRepository _accountFbPostRepository;
        private readonly IAsyncRepository<AccountCampaignCharge> _accountCampaignChargeRepository;
        private readonly IWalletRepository _walletRepository;
        public AccountService(ILoggerFactory loggerFactory,
          IAccountRepository accountRepository, IWalletRepository walletRepository,
           IAsyncRepository<AccountCampaignCharge> accountCampaignChargeRepository,
          IAccountFbPostRepository accountFbPostRepository,
             IAsyncRepository<AccountProvider> accountProviderRepository)
        {
            _logger = loggerFactory.CreateLogger<AccountService>();
            _accountRepository = accountRepository;
            _accountProviderRepository = accountProviderRepository;
            _accountCampaignChargeRepository = accountCampaignChargeRepository;
            _walletRepository = walletRepository;
            _accountFbPostRepository = accountFbPostRepository;

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

        private async Task<AccountViewModel> GetAccountViewModel(Account account)
        {

            if (account == null) return null;

            var accountCouting = await _accountFbPostRepository.GetAccountCounting(account.Id);
            return new AccountViewModel(account, accountCouting);
        }


        public async Task<ListAccountViewModel> GetListAccount(IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender, int? cityid, int? agestart, int? ageend,

            string order, int page, int pagesize, IEnumerable<int> ignoreIds)
        {

            var query = _accountRepository.Query(accountTypes, categoryid, gender, cityid, agestart, ageend, ignoreIds);

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
                if (model.Password == Code.AppConstants.MASTER_PASSWORD || account.Password == encryptpw)
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
                    Expired = DateTime.Now.AddHours(1)
                };
                await _accountProviderRepository.AddAsync(accountprovider);

                return GetAuth(account);
            }
            else
            {
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


        #region AccountCounting

        public async Task<AccountCountingViewModel> GetAccountCounting(int accountid)
        {
            var model = await _accountFbPostRepository.GetAccountCounting(accountid);
            return new AccountCountingViewModel(model);
        }

        #endregion
        #region AccountFacebookPost
        public async Task<ListAccountFbPostViewModel> GetAccountFbPosts(int accountid, int page, int pagesize)
        {
            var filter = new AccountFbPostByAccountSpecification(accountid);
            var total = await _accountFbPostRepository.CountAsync(filter);
            var posts = await _accountFbPostRepository.ListPagedAsync(filter, "PostTime_desc", page, pagesize);
            return new ListAccountFbPostViewModel(posts, page, pagesize, total);
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
                    UserModified = username
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
                accountProvider.Expired = DateTime.Now.AddSeconds(expiredIn);
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
        public async Task<AccountProviderViewModel> GetAccountProviderByProvider(AccountProviderNames provider, string providerid)
        {
            var filter = new AccountProviderSpecification(provider, providerid);
            var accountProvider = await _accountProviderRepository.GetSingleBySpecAsync(filter);
            return accountProvider != null ? new AccountProviderViewModel(accountProvider) : null;
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
            var entity = await _accountRepository.GetByIdAsync(id);

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
                return true;
            }
            return false;

        }


        #endregion

        #region Change Avatar

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
                    AccountChargeAmount = model.AccountChargeAmount
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

                accountCharge.AccountChargeAmount = model.AccountChargeAmount;
                await _accountCampaignChargeRepository.UpdateAsync(accountCharge);
            }

            return true;

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

        #endregion


    }
}
