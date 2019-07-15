
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

        #region Campaigns
        public async Task<List<int>> GetEndedCampaignIds()
        {
            return await _campaignRepository.GetCampaignIds(CampaignStatus.Ended);
        }

        public async Task<List<int>> GetFinishedAccountIdsByCampaignId(int campaignid)
        {
            var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaignid, CampaignAccountStatus.Finished));


            return campaignAccounts.Select(m => m.AccountId).ToList();
        }
        #endregion

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



        public async Task<ListCampaignViewModel> GetListCampaignByAgency(int agencyid, CampaignType? type, CampaignStatus? status, string keyword, int page, int pagesize)
        {
            var filter = new CampaignByAgencySpecification(agencyid, type, status, keyword);
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


        public async Task<CreateCampaignViewModel> GetCreateCampaign(int agencyid)
        {
            var code = await _campaignRepository.GetValidCode(agencyid);

            return new CreateCampaignViewModel()
            {
                Code = code
            };
        }

        public async Task<int> CreateCampaign(int agencyid, CreateCampaignViewModel model, string username)
        {
            var campaignTypeCharge = await _campaignTypeChargeRepository.GetSingleBySpecAsync(new CampaignTypeChargeSpecification(model.Type));
            if (campaignTypeCharge == null)
            {
                return -1;
            }
            var settings = await _settingRepository.GetSetting();
            // var wallet = await _walletRepository.GetBalance(EntityType.Agency, agencyid);
            var code = await _campaignRepository.GetValidCode(agencyid);
            var campaign = model.GetEntity(agencyid, campaignTypeCharge, settings, code, username);
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
        public async Task<bool> RequestJoinCampaignByAgency(int agencyid, int campaignid, int accountid, int amount, string username)
        {
            var createStatus = await _campaignAccountRepository.CreateAgencyRequestCampaignAccount(agencyid, campaignid, accountid, amount, username);

            if (createStatus > 0)
            {
                //add notification
                await _notificationRepository.CreateNotification(NotificationType.AgencyRequestJoinCampaign, EntityType.Account, accountid, campaignid,
                     NotificationType.AgencyRequestJoinCampaign.GetMessageText(username, campaignid.ToString()));
                return true;

            }
            return false;
        }

        public async Task<bool> FeedbackJoinCampaignByAccount(int accountid, int campaignid, string username, bool confirmed)
        {

            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            if (campaign != null)
            {
                var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(new CampaignAccountByAccountSpecification(accountid, campaignid));

                if (campaignAccount != null)
                {
                    if (campaignAccount.Status == CampaignAccountStatus.AgencyRequest)
                    {
                        campaignAccount.Status = confirmed ? CampaignAccountStatus.Confirmed : CampaignAccountStatus.Canceled;
                        campaignAccount.DateModified = DateTime.Now;
                        campaignAccount.UserModified = username;
                        await _campaignAccountRepository.UpdateAsync(campaignAccount);

                        var notifType = confirmed ? NotificationType.AccountConfirmJoinCampaign : NotificationType.AccountDeclineJoinCampaign;
                        await _notificationRepository.AddAsync(new Notification()
                        {
                            Type = notifType,
                            DataId = campaign.Id,
                            Data = string.Empty,
                            DateCreated = DateTime.Now,
                            EntityType = EntityType.Agency,
                            EntityId = campaign.AgencyId,
                            Message = notifType.GetMessageText(username, campaign.Id.ToString()),
                            Status = NotificationStatus.Created
                        });

                        return true;
                    }

                }
            }

            return false;
        }


        public async Task<bool> FeedbackJoinCampaignByAgency(int agencyid, int campaignid, int accountid, bool confirmed, string username)
        {

            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            if (campaign != null || campaign.AgencyId != agencyid)
            {
                var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(new CampaignAccountByAccountSpecification(accountid, campaignid));

                if (campaignAccount != null)
                {
                    if (campaignAccount.Status == CampaignAccountStatus.AccountRequest || campaignAccount.Status == CampaignAccountStatus.AgencyRequest)
                    {
                        var notifType = NotificationType.AgencyConfirmJoinCampaign;
                        if (confirmed)
                        {
                            campaignAccount.Status = CampaignAccountStatus.Confirmed;
                        }
                        else
                        {
                            notifType = NotificationType.AgencyCancelAccountJoinCampaign;
                            campaignAccount.Status = CampaignAccountStatus.Canceled;
                        }

                        campaignAccount.DateModified = DateTime.Now;
                        campaignAccount.UserModified = username;
                        await _campaignAccountRepository.UpdateAsync(campaignAccount);

                        await _notificationRepository.AddAsync(new Notification()
                        {
                            Type = notifType,
                            DataId = campaign.Id,
                            Data = string.Empty,
                            DateCreated = DateTime.Now,
                            EntityType = EntityType.Account,
                            EntityId = accountid,
                            Message = notifType.GetMessageText(username, campaign.Id.ToString()),
                            Status = NotificationStatus.Created
                        });

                        return true;
                    }

                }
            }

            return false;
        }


        #endregion

        #region Action


        public async Task<bool> UpdateCampaignCompleted(int campaignid, string username)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            if (campaign != null && campaign.Status == CampaignStatus.Ended)
            {
                campaign.Status = CampaignStatus.Completed;
                campaign.UserModified = username;
                campaign.DateModified = DateTime.Now;

                await _campaignRepository.UpdateAsync(campaign);

                return true;
            }

            return false;

        }

        public async Task<bool> UpdateCampaignError(int campaignid, string note, string username)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            if (campaign != null && campaign.Status == CampaignStatus.Ended)
            {
                campaign.Status = CampaignStatus.Error;
                campaign.SystemNote = note;
                campaign.UserModified = username;
                campaign.DateModified = DateTime.Now;

                await _campaignRepository.UpdateAsync(campaign);

                return true;
            }

            return false;

        }
        
        public async Task<int> UpdateCampaignStatusByAgency(int agencyid, int campaignid, CampaignStatus status, string username)
        {

            var campaign = await _campaignRepository.GetSingleBySpecAsync(new CampaignByAgencySpecification(agencyid, campaignid));
            if (campaign != null)
            {
                if (status == CampaignStatus.Canceled && (campaign.Status != CampaignStatus.AddAccount || campaign.Status == CampaignStatus.WaitToConfirm))
                {
                    return -1;
                }
                if (status == CampaignStatus.AddAccount && campaign.Status != CampaignStatus.WaitToConfirm)
                {
                    return -1;
                }

                if (status == CampaignStatus.Started && campaign.Status != CampaignStatus.AddAccount)
                {
                    return -1;
                }
                if (status == CampaignStatus.Started && campaign.Status != CampaignStatus.AddAccount)
                {
                    return -1;
                }

                if (status == CampaignStatus.Ended && campaign.Status != CampaignStatus.Started)
                {
                    return -1;
                }
                if (status == CampaignStatus.Completed && campaign.Status != CampaignStatus.Ended)
                {
                    return -1;
                }

                if (status == CampaignStatus.Ended)
                {
                    //check so luong nguoi hoan thanh
                    var count = await _campaignAccountRepository.CountAsync(new CampaignAccountSpecification(campaignid, null
                        , new List<CampaignAccountStatus> {
                            CampaignAccountStatus.Finished,
                            CampaignAccountStatus.Canceled
                        }));

                    if (count > 0)
                    {
                        return -2;
                    }

                }
                else if (status == CampaignStatus.Started)
                {
                    var count2 = await _campaignAccountRepository.CountAsync(new CampaignAccountSpecification(campaignid, null, new List<CampaignAccountStatus>() {

                            CampaignAccountStatus.Canceled,
                    }));

                    if (count2 == 0)
                    {
                        return -3;
                    }

                    var count = await _campaignAccountRepository.CountAsync(new CampaignAccountSpecification(campaignid, new List<CampaignAccountStatus> {
                            CampaignAccountStatus.AccountRequest,
                            CampaignAccountStatus.AgencyRequest,
                    }, null));

                    if (count > 0)
                    {
                        return -3;
                    }
                }





                if (status == CampaignStatus.Started)
                {
                    campaign.DateStart = DateTime.Now;
                }
                if (status == CampaignStatus.Ended)
                {
                    campaign.DateEnd = DateTime.Now;
                }


                campaign.Status = status;
                campaign.UserModified = username;
                campaign.DateModified = DateTime.Now;

                await _campaignRepository.UpdateAsync(campaign);


                if (status == CampaignStatus.Started)
                {

                }

                return 1;
            }

            return 0;
        }


        public async Task<int> SubmmitCampaignAccountChangeAvatar(int accountid, SubmmitCampaignAccountChangeAvatarViewModel model, string username)
        {

            var campaign = await _campaignRepository.GetByIdAsync(model.CampaignId);
            if (campaign == null) return -1;

            var filter = new CampaignAccountByAccountSpecification(accountid, campaign.Id);
            var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(filter);
            if (campaignAccount == null) return -1;

            campaignAccount.Status = CampaignAccountStatus.Finished;
            campaignAccount.DateModified = DateTime.Now;
            campaignAccount.UserModified = username;
            await _campaignAccountRepository.UpdateAsync(campaignAccount);


            //notification

            await _notificationRepository.CreateNotification(NotificationType.AccountFinishCampaignRefContent,
                EntityType.Agency, campaign.AgencyId, campaign.Id,
                 NotificationType.AccountFinishCampaignRefContent.GetMessageText(username, campaign.Id.ToString()),
                 campaignAccount.Id.ToString());
            return 1;
        }




        public async Task<int> UpdateCampaignAccountRef(int accountid, UpdateCampaignAccountRefViewModel model, string username)
        {
            var campaign = await _campaignRepository.GetByIdAsync(model.CampaignId);
            if (campaign == null) return -1;

            var filter = new CampaignAccountByAccountSpecification(accountid, campaign.Id);
            var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(filter);
            if (campaignAccount == null) return -1;

            if (!string.IsNullOrEmpty(model.RefId))
            {
                campaignAccount.RefId = model.RefId;
            }
            campaignAccount.RefUrl = model.RefUrl;


            campaignAccount.Status = CampaignAccountStatus.Finished;
            campaignAccount.DateModified = DateTime.Now;
            campaignAccount.UserModified = username;
            await _campaignAccountRepository.UpdateAsync(campaignAccount);

            //notification

            await _notificationRepository.CreateNotification(NotificationType.AccountFinishCampaignRefContent,
                EntityType.Agency, campaign.AgencyId, campaign.Id,
                 NotificationType.AccountFinishCampaignRefContent.GetMessageText(username, campaign.Id.ToString()),
                 campaignAccount.Id.ToString());
            return 1;
        }

        public async Task<int> SubmitCampaignAccountRefContent(int accountid, SubmitCampaignAccountRefContentViewModel model, string username)
        {
            // thiều phần verify .... 
            var campaign = await _campaignRepository.GetByIdAsync(model.CampaignId);
            if (campaign == null) return -1;

            var filter = new CampaignAccountByAccountSpecification(accountid, campaign.Id);
            var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(filter);
            if (campaignAccount == null) return -1;


            campaignAccount.RefContent = model.RefContent;

            campaignAccount.Status = CampaignAccountStatus.Submitted;
            campaignAccount.DateModified = DateTime.Now;
            campaignAccount.UserModified = username;
            await _campaignAccountRepository.UpdateAsync(campaignAccount);

            //notification

            await _notificationRepository.CreateNotification(NotificationType.AccountSubmitCampaignRefContent,
                EntityType.Agency, campaign.AgencyId, campaign.Id,
                 NotificationType.AccountSubmitCampaignRefContent.GetMessageText(username, campaign.Id.ToString()),
                 campaignAccount.Id.ToString());

            return 1;
        }


        public async Task<int> FeedbackCampaignAccountRefContent(int agencyid, int campaignid, int accountid, string username, int type, string newContent)
        {

            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            if (campaign == null || campaign.AgencyId != agencyid) return -1;

            var filter = new CampaignAccountByAccountSpecification(accountid, campaign.Id);
            var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(filter);
            if (campaignAccount == null) return -1;


            campaignAccount.Status = type == 1 ? CampaignAccountStatus.Approved : CampaignAccountStatus.Declined;
            campaignAccount.DateModified = DateTime.Now;
            campaignAccount.UserModified = username;
            await _campaignAccountRepository.UpdateAsync(campaignAccount);

            //notification
            var notiftype = type == 1 ? NotificationType.AgencyApproveCampaignRefContent : NotificationType.AgencyDeclineCampaignRefContent;
            await _notificationRepository.CreateNotification(notiftype,
                EntityType.Account, campaignAccount.AccountId, campaign.Id,
                 notiftype.GetMessageText(username, campaign.Id.ToString()),
                 campaignAccount.Id.ToString());

            return 1;
        }



        #endregion

        #region Campaign Types
        public async Task<List<CampaignTypeChargeViewModel>> GetCampaignTypeCharges()
        {
            var list = await _campaignTypeChargeRepository.ListAllAsync();
            return CampaignTypeChargeViewModel.GetList(list);
        }
        #endregion
    }
}
