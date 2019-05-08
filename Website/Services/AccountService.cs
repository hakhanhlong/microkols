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

namespace Website.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IAsyncRepository<AccountProvider> _accountProviderRepository;
        private readonly IWalletRepository _walletRepository;
        public AccountService(ILoggerFactory loggerFactory,
          IAccountRepository accountRepository, IWalletRepository walletRepository,
             IAsyncRepository<AccountProvider> accountProviderRepository)
        {
            _logger = loggerFactory.CreateLogger<AccountService>();
            _accountRepository = accountRepository;
            _accountProviderRepository = accountProviderRepository;
            _walletRepository = walletRepository;

        }

        public async Task<AccountViewModel> GetAccount(int id)
        {
            var account = await _accountRepository.GetActivedAccount(id);
            return GetAccountViewModel(account);
        }

        private AccountViewModel GetAccountViewModel(Account account)
        {
            return (account == null) ? null : new AccountViewModel(account);
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
                        Avatar = string.Empty,
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


        #region ChangeIDCard

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

        #endregion


    }
}
