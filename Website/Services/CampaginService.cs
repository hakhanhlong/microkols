
using Common.Extensions;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.ViewModels;

namespace Website.Services
{
    public class CampaignService : BaseService, ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IAsyncRepository<CampaignTypeCharge> _campaignTypeChargeRepository;
        private readonly IAsyncRepository<CampaignOption> _campaignOptionRepository;
        private readonly IAsyncRepository<CampaignAccountType> _campaignAccountTypeRepository;
        private readonly ICampaignAccountRepository _campaignAccountRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly INotificationRepository _notificationRepository;

        public CampaignService(ICampaignRepository campaignRepository,
            ITransactionRepository transactionRepository,
            IWalletRepository walletRepository,
            IAsyncRepository<CampaignTypeCharge> campaignTypeChargeRepository,
            IAsyncRepository<CampaignOption> campaignOptionRepository,
            IAsyncRepository<CampaignAccountType> campaignAccountTypeRepository,
           ICampaignAccountRepository campaignAccountRepository,
           INotificationRepository notificationRepository,
             ISettingRepository settingRepository)
        {
            _campaignAccountTypeRepository = campaignAccountTypeRepository;
            _campaignTypeChargeRepository = campaignTypeChargeRepository;
            _campaignOptionRepository = campaignOptionRepository;
            _campaignRepository = campaignRepository;
            _walletRepository = walletRepository;
            _settingRepository = settingRepository;
            _transactionRepository = transactionRepository;
            _campaignAccountRepository = campaignAccountRepository;
            _notificationRepository = notificationRepository;
        }


        #region Campaign By Account
        public async Task<ListCampaignWithAccountViewModel> GetListCampaignByAccount(int accountid, string keyword, int page, int pagesize)
        {
            var filter = new CampaignByAccountSpecification(accountid, keyword);
            var campaigns = await _campaignRepository.ListPagedAsync(filter, "", page, pagesize);
            var total = await _campaignRepository.CountAsync(filter);


            var list = new List<CampaignWithAccountViewModel>();

            foreach (var campaign in campaigns)
            {
                var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(new CampaignAccountByAccountSpecification(accountid, campaign.Id));
                if (campaignAccount != null)
                {
                    list.Add(new CampaignWithAccountViewModel(campaign, campaignAccount));
                }
            }

            return new ListCampaignWithAccountViewModel()
            {
                Campaigns = list,
                Pager = new PagerViewModel(page, pagesize, total)
            };
        }
        public async Task<CampaignDetailsViewModel> GetCampaignDetailsByAccount(int accountid, int id)
        {
            var filter = new CampaignByAccountSpecification(accountid, id);
            var campaign = await _campaignRepository.GetSingleBySpecAsync(filter);
            if (campaign != null)
            {
                return new CampaignDetailsViewModel(campaign, campaign.CampaignOption,
                    campaign.CampaignAccount, new List<Transaction>());
            }
            return null;
        }

        #endregion


        #region Campaign By Agency



        public async Task<ListCampaignViewModel> GetListCampaignByAgency(int agencyid, CampaignType? type, string keyword, int page, int pagesize)
        {
            var filter = new CampaignByAgencySpecification(agencyid, type, keyword);
            var campaigns = await _campaignRepository.ListPagedAsync(filter, "DateModified_desc", page, pagesize);
            var total = await _campaignRepository.CountAsync(filter);

            return new ListCampaignViewModel()
            {
                Campaigns = CampaignViewModel.GetList(campaigns),
                Pager = new PagerViewModel(page, pagesize, total)
            };
        }

        public async Task<CampaignDetailsViewModel> GetCampaignDetailsByAgency(int agencyid, int id)
        {
            var filter = new CampaignByAgencySpecification(agencyid, id);
            var campaign = await _campaignRepository.GetSingleBySpecAsync(filter);
            if (campaign != null)
            {
                var transactions = await _transactionRepository.ListAsync(new TransactionByCampaignSpecification(campaign.Id));
                return new CampaignDetailsViewModel(campaign, campaign.CampaignOption,
                    campaign.CampaignAccount, transactions);
            }
            return null;
        }

        public async Task<CampaignPaymentModel> GetCampaignPaymentByAgency(int agencyid, int id)
        {
            return await _campaignRepository.GetCampaignPaymentByAgency(agencyid, id);
        }


        public async Task<int> CreateCampaign(int agencyid, CreateCampaignViewModel model, string username)
        {
            var campaignTypeCharge = await _campaignTypeChargeRepository.GetSingleBySpecAsync(new CampaignTypeChargeSpecification(model.Type));
            if (campaignTypeCharge == null)
            {
                return -1;
            }
            var settings = await _settingRepository.GetSetting();
            var wallet = await _walletRepository.GetBalance(EntityType.Agency, agencyid);
            var campaign = model.GetEntity(agencyid, campaignTypeCharge, settings, username);
            if (campaign == null)
            {
                return -1;
            }

            await _campaignRepository.AddAsync(campaign);
            if (campaign.Id > 0)
            {
                await CreateCampaignAccountType(campaign.Id, model.AccountType, username);

                await CreateCampaignOptions(campaign.Id, model, username);

                return campaign.Id;

            }



            return -1;
        }
        private async Task CreateCampaignAccountType(int campaignId, IEnumerable<AccountType> accountTypes, string username)
        {
            foreach (var accountType in accountTypes)
            {
                await _campaignAccountTypeRepository.AddAsync(new CampaignAccountType()
                {
                    AccountType = accountType,
                    CampaignId = campaignId
                });
            }

        }
        private async Task CreateCampaignOptions(int campaignId, CreateCampaignViewModel model, string username)
        {
            if (model.EnabledCity && model.CityId.HasValue)
            {
                await _campaignOptionRepository.AddAsync(new CampaignOption()
                {
                    CampaignId = campaignId,
                    Name = CampaignOptionName.City,
                    Value = model.CityId.Value.ToString()
                });
            }
            if (model.EnabledGender && model.Gender.HasValue)
            {
                await _campaignOptionRepository.AddAsync(new CampaignOption()
                {
                    CampaignId = campaignId,
                    Name = CampaignOptionName.Gender,
                    Value = model.Gender.Value.ToString()
                });
            }

            if (model.EnabledAgeRange && model.AgeEnd.HasValue && model.AgeStart.HasValue)
            {
                await _campaignOptionRepository.AddAsync(new CampaignOption()
                {
                    CampaignId = campaignId,
                    Name = CampaignOptionName.AgeRange,
                    Value = $"{model.AgeStart.Value}-{model.AgeEnd.Value}"
                });
            }

            if (model.EnabledCategory && model.CategoryId != null && model.CategoryId.Count > 0)
            {
                foreach (var categoryid in model.CategoryId)
                {
                    await _campaignOptionRepository.AddAsync(new CampaignOption()
                    {
                        CampaignId = campaignId,
                        Name = CampaignOptionName.Category,
                        Value = categoryid.ToString()
                    });
                }

            }

        }

        #endregion


        #region Campaign Request


        #endregion

        #region Campaign Account
        public async Task<CampaignAccountViewModel> GetCampaignAccountByAccount(int accountid, int campaignid)
        {
            var filter = new CampaignAccountByAccountSpecification(accountid, campaignid);
            var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(filter);

            return campaignAccount != null ? new CampaignAccountViewModel(campaignAccount) : null;

        }
        public async Task<ListCampaignAccountViewModel> GetCampaignAccounts(int campaignid, int page, int pagesize)
        {
            var filter = new CampaignAccountByAgencySpecification(campaignid);

            var total = await _campaignAccountRepository.CountAsync(filter);
            var campaignAccounts = await _campaignAccountRepository.ListPagedAsync(filter, string.Empty, page, pagesize);

            return new ListCampaignAccountViewModel(campaignAccounts, total, page, pagesize);


        }
        public async Task<bool> RequestJoinCampaignByAgency(int agencyid, int campaignid, int accountid, string username)
        {
            var createStatus = await _campaignAccountRepository.CreateAgencyRequestCampaignAccount(agencyid, campaignid, accountid, username);

            if (createStatus > 0)
            {
                //add notification
                await _notificationRepository.AddAsync(new Notification()
                {
                    Type = NotificationType.AgencyRequestJoinCampaign,
                    EntityType = EntityType.Account,
                    EntityId = accountid,
                    DataId = campaignid,
                    Data = string.Empty,
                    Image = string.Empty,
                    Status = NotificationStatus.Created,
                    DateCreated = DateTime.Now,
                    Message = NotificationType.AgencyRequestJoinCampaign.GetMessageText(username, campaignid.ToString())
                });
                return true;

            }
            return false;
        }

        public async Task<bool> ConfirmJoinCampaignByAccount(int accountid, int campaignid, string username)
        {

            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            if (campaign != null)
            {
                var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(new CampaignAccountByAccountSpecification(accountid, campaignid));

                if (campaignAccount != null)
                {
                    if (campaignAccount.Status == CampaignAccountStatus.AgencyRequest)
                    {
                        campaignAccount.Status = CampaignAccountStatus.Confirmed;
                        campaignAccount.DateModified = DateTime.Now;
                        campaignAccount.UserModified = username;
                        await _campaignAccountRepository.UpdateAsync(campaignAccount);

                        await _notificationRepository.AddAsync(new Notification()
                        {
                            Type = NotificationType.AccountConfirmJoinCampaign,
                            DataId = campaign.Id,
                            Data = string.Empty,
                            DateCreated = DateTime.Now,
                            EntityType = EntityType.Agency,
                            EntityId = campaign.AgencyId,
                            Message = NotificationType.AccountConfirmJoinCampaign.GetMessageText(username, campaign.Id.ToString()),
                            Status = NotificationStatus.Created
                        });

                        return true;
                    }

                }
            }

            return false;
        }

        #endregion

        public async Task<List<CampaignTypeChargeViewModel>> GetCampaignTypeCharges()
        {
            var list = await _campaignTypeChargeRepository.ListAllAsync();
            return CampaignTypeChargeViewModel.GetList(list);
        }
    }
}
