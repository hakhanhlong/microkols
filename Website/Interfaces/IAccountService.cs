using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.ViewModels;

namespace Website.Interfaces
{
    public interface IAccountService
    {
        Task<ListAccountViewModel> GetListAccount(IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender, int? cityid, int? agestart, int? ageend,
                    string order, int page, int pagesize, IEnumerable<int> ignoreIds);

        Task<bool> ChangeAvatar(int id, ChangeAvatarViewModel model);
        Task<bool> ChangeContact(int id, ChangeContactViewModel model, string username);
        Task<bool> ChangeIDCard(int id, ChangeIDCardViewModel model, string username);
        Task<bool> ChangeInformation(int id, ChangeInformationViewModel model, string username);
        Task<bool> ChangePassword(int id, ChangePasswordViewModel model, string username);

        Task<ChangeAccountTypeViewModel> GetChangeAccountType(int id);
        Task<bool> ChangeAccountType(int id, ChangeAccountTypeViewModel model, string username);

        Task<ForgotPasswordResultViewModel> ForgotPassword(string email);
        Task<AccountViewModel> GetAccount(int id);

        Task<int> Register(RegisterViewModel model);
        Task<AuthViewModel> GetAuth(LoginViewModel model);
        Task<AuthViewModel> GetAuth(LoginProviderViewModel model);
        Task<AuthViewModel> GetAuth(int id);

        Task<ChangeContactViewModel> GetContact(int id);
        Task<ChangeIDCardViewModel> GetIDCard(int id);
        Task<ChangeInformationViewModel> GetInformation(int id);
        Task<bool> VerifyIDCard(string email);

        Task<ChangeBankAccountViewModel> GetBankAccount(int id);
        Task<bool> ChangeBankAccount(int id, ChangeBankAccountViewModel model, string username);
        Task<bool> VerifyEmail(string email);


        Task<int> UpdateAccountProvider(int accountid, UpdateAccountProviderViewModel model, string username);

        Task<List<AccountCampaignChargeViewModel>> GetAccountCampaignCharges(int accountid);
        Task<bool> UpdateAccountCampaignCharge(int accountid, AccountCampaignChargeViewModel model);


        Task<string> GetProviderIdByAccount(int accountid, AccountProviderNames provider);

    }
}
