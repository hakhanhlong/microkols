using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
    public interface IAccountService
    {
        Task<ChangeFacebookUrlViewModel> GetChangeFacebookUrl(int id);
        Task<bool> ChangeFacebookUrl(int id, ChangeFacebookUrlViewModel model);

        Task<bool> ChangeAvatar(int id, string path, string username);
        Task<AccountStatus> GetAccountStatus(int id);
        Task<string> GetAccessToken(int id);
        Task CreateDumbAccount(int count = 30);
        Task<AccountCountingViewModel> GetAccountCounting(int accountid);
        Task<List<int>> GetActivedAccountIds();
        Task<ListAccountViewModel> GetListAccount(IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender, IEnumerable<int> cityid, int? agestart, int? ageend,
                    string order, int page, int pagesize, IEnumerable<int> ignoreIds, int min, int max);
        Task<AccountFbPostViewModel> GetAccountFbPost(int id);
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
        Task<string> GetFacebookProfileUrl(int id);
        Task<AuthViewModel> GetAuth(LoginProviderViewModel model);
        Task<AuthViewModel> GetAuth(int id);

        Task<ChangeContactViewModel> GetContact(int id);
        Task<ChangeIDCardViewModel> GetIDCard(int id);
        Task<ChangeInformationViewModel> GetInformation(int id);
        Task<bool> VerifyIDCard(string email);

        Task<ChangeBankAccountViewModel> GetBankAccount(int id);
        Task<bool> ChangeBankAccount(int id, ChangeBankAccountViewModel model, string username);
        Task<bool> VerifyEmail(string email);

        Task<ListAccountViewModel> GetMatchedAccountByCampaignId(int campaignid,  string order, int page, int pagesize);

        Task<bool> UpdateAccountProviderInfo(int accountproviderid, string link, int friendscount, string username);
        Task<List<AccountCampaignChargeViewModel>> GetAccountCampaignCharges(int accountid);
        Task<bool> UpdateAccountCampaignCharge(int accountid, AccountCampaignChargeViewModel model);
        Task<int> GetAcountChargeAmount(int accountid, CampaignType campaignType);


        #region Account Provider
        Task<AccountProviderViewModel> GetAccountProviderByProvider(AccountProviderNames provider, string providerid, string newtoken);
        Task<List<AccountProviderViewModel>> GetAccountProvidersByExpiredToken(AccountProviderNames provider);
        Task<int> UpdateAccountProvider(int accountid, UpdateAccountProviderViewModel model, string username);
        Task<AccountProviderViewModel> GetAccountProviderByAccount(int accountid, AccountProviderNames provider);
        Task<string> GetProviderIdByAccount(int accountid, AccountProviderNames provider);
        Task<bool> UpdateAccountProvidersAccessToken(int id, string accessToken, int expiredIn);

        #endregion

        #region Account FbPost
        Task<AccountFbPostViewModel> GetAccountFbPost(int accountid, string postid);
        Task UpdateFbPost(int accountid, AccountFbPostViewModel model, string username);
        Task<ListAccountFbPostViewModel> GetAccountFbPosts(int accountid, int type, int page, int pagesize);
        #endregion


        Task<List<CampaignType>> GetIgnoreCampaignTypes(int accountid);
        Task<bool> UpdateIgnoreCampaignTypes(int accountid, CampaignType type, bool removed, string username);

        Task<List<AccountViewModel>> GetAccounts(AccountType type, string kw, string order, int page, int pagesize);
    }
}
