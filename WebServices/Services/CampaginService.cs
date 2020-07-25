
using Common.Extensions;
using Common.Helpers;
using Core.Entities;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Hangfire;
using Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Interfaces;
using WebServices.ViewModels;

namespace WebServices.Services
{
    public class CampaignService : BaseService, ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IAsyncRepository<CampaignTypeCharge> _campaignTypeChargeRepository;
        private readonly IAsyncRepository<CampaignAccountCaption> _campaignAccountCaptionRepository;
        private readonly IAsyncRepository<CampaignAccountContent> _campaignAccountContentRepository;


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
            IAsyncRepository<CampaignAccountCaption> campaignAccountCaptionRepository,
            IAsyncRepository<CampaignAccountContent> campaignAccountContentRepository,
            INotificationRepository notificationRepository,
             ISettingRepository settingRepository)
        {
            _campaignAccountTypeRepository = campaignAccountTypeRepository;
            _campaignTypeChargeRepository = campaignTypeChargeRepository;
            _campaignOptionRepository = campaignOptionRepository;
            _campaignRepository = campaignRepository;
            _walletRepository = walletRepository;
            _settingRepository = settingRepository;
            _campaignAccountCaptionRepository = campaignAccountCaptionRepository;
            _campaignAccountContentRepository = campaignAccountContentRepository;
            _transactionRepository = transactionRepository;
            _campaignAccountRepository = campaignAccountRepository;
            _notificationRepository = notificationRepository;
        }

        #region Campaigns

        public async Task<CampaignViewModel> GetCampaign(int id)
        {
            var campaign = await _campaignRepository.GetByIdAsync(id);
            if (campaign != null) { return new CampaignViewModel(campaign); }
            return null;
        }

        public async Task<CampaignViewModel> GetCampaignById(int id)
        {
            var filter = new CampaignSpecification(id);
            var campaign = await _campaignRepository.GetSingleBySpecAsync(filter);
            if (campaign != null) { return new CampaignViewModel(campaign); }
            return null;
        }

        public async Task<List<int>> GetEndedCampaignIds()
        {
            return await _campaignRepository.GetCampaignIds(CampaignStatus.Ended);
        }

        //############ addition by longhk ##########################################
        public async Task<List<int>> GetLockedCampaignIds()
        {
            return await _campaignRepository.GetCampaignIds(CampaignStatus.Locked);
        }
        //##########################################################################

        public async Task<List<int>> GetFinishedAccountIdsByCampaignId(int campaignid)
        {
            var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaignid, CampaignAccountStatus.Finished));


            return campaignAccounts.Select(m => m.AccountId).ToList();
        }



        #endregion

        public async Task<CampaignAccountCountingViewModel> GetCampaignAccountCounting(int campaignid, CampaignType type, int total)
        {

            var totalRequest = await _campaignAccountRepository.CountAsync(new CampaignAccountByAgencySpecification(campaignid, new List<CampaignAccountStatus>() {
                CampaignAccountStatus.AccountRequest,
                CampaignAccountStatus.AgencyRequest,
                CampaignAccountStatus.ApprovedContent,
                CampaignAccountStatus.Confirmed,
                CampaignAccountStatus.DeclinedContent,
                CampaignAccountStatus.Finished,
                CampaignAccountStatus.SubmittedContent,
                CampaignAccountStatus.UpdatedContent,
                CampaignAccountStatus.WaitToPay,
            }));

            if (type.IsHasCaption())
            {
                var tongcaption = await _campaignAccountCaptionRepository.CountAsync(new CampaignAccountCaptionByCampaignIdSpecification(campaignid));

                var tongcaptionDaduyet = await _campaignAccountCaptionRepository.CountAsync(new CampaignAccountCaptionByCampaignIdSpecification(campaignid, CampaignAccountCaptionStatus.DaDuyet));


                var tongcaptionChuaDuyet = await _campaignAccountCaptionRepository.CountAsync(new CampaignAccountCaptionByCampaignIdSpecification(campaignid, CampaignAccountCaptionStatus.ChoDuyet));




                return new CampaignAccountCountingViewModel()
                {
                    TongCaption = tongcaption,
                    TongCaptionCanDuyet = tongcaptionChuaDuyet,
                    TongCaptionDaDuyet = tongcaptionDaduyet,
                    TongNguoiThamGia = totalRequest,
                    TongNguoi = total,
                };
            }
            else if (type.IsHasContent())
            {
                var tongContent = await _campaignAccountContentRepository.CountAsync(new CampaignAccountContentByCampaignIdSpecification(campaignid));

                var tongContentDaduyet = await _campaignAccountContentRepository.CountAsync(new CampaignAccountContentByCampaignIdSpecification(campaignid, CampaignAccountContentStatus.DaDuyet));


                var tongContentChuaDuyet = await _campaignAccountContentRepository.CountAsync(new CampaignAccountContentByCampaignIdSpecification(campaignid, CampaignAccountContentStatus.ChoDuyet));




                return new CampaignAccountCountingViewModel()
                {
                    TongContent = tongContent,
                    TongContentDaDuyet = tongContentDaduyet,
                    TongContentCanDuyet = tongContentChuaDuyet,
                    TongNguoiThamGia = totalRequest,
                    TongNguoi = total,
                };
            }

            return new CampaignAccountCountingViewModel()
            {
                TongNguoiThamGia = totalRequest,
                TongNguoi = total,
            };
        }
        public async Task<string> GetCampaignCode(int id)
        {
            var campaign = await _campaignRepository.GetByIdAsync(id);

            return campaign != null ? campaign.Code : string.Empty;

        }


        #region Campaign By Account
        public async Task<ListCampaignWithAccountViewModel> GetListCampaignByAccount(int accountid, int type, string keyword, int page, int pagesize)
        {

            var query = await _campaignRepository.QueryCampaignByAccount(accountid, type, keyword);

            var total = await query.CountAsync();
            var campaigns = await query.OrderByDescending(m => m.Id).GetPagedAsync(page, pagesize);

            //var filter = new CampaignByAccountSpecification(accountid, keyword);
            //var campaigns = await _campaignRepository.ListPagedAsync(filter, "", page, pagesize);
            //var total = await _campaignRepository.CountAsync(filter);


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
        public async Task<ListMarketPlaceViewModel> GetCampaignMarketPlaceByAccount(int accountid, CampaignType? type, string keyword, int page, int pagesize)
        {

            var query = await _campaignRepository.QueryMarketPlaceCampaignByAccount(accountid, type, keyword);

            var total = await query.CountAsync();
            var campaigns = await query.OrderByDescending(m => m.Id).GetPagedAsync(page, pagesize);

            //var filter = new CampaignByAccountSpecification(accountid, keyword);
            //var campaigns = await _campaignRepository.ListPagedAsync(filter, "", page, pagesize);
            //var total = await _campaignRepository.CountAsync(filter);


            var list = new List<MarketPlaceViewModel>();

            foreach (var campaign in campaigns)
            {
                var campaignAccount = await _campaignAccountRepository.ListAsync(new CampaignAccountByAgencySpecification(campaign.Id));

                list.Add(new MarketPlaceViewModel(campaign, campaignAccount, campaign.Agency));

            }

            return new ListMarketPlaceViewModel()
            {
                MarketPlaces = list,
                Pager = new PagerViewModel(page, pagesize, total)
            };
        }

        public async Task<MarketPlaceViewModel> GetCampaignMarketPlace(int id)
        {
            var campaign = await _campaignRepository.GetSingleBySpecAsync(new CampaignSpecification(id));

            if (campaign == null) return null;
            return new MarketPlaceViewModel(campaign, campaign.CampaignAccount.ToList(), campaign.Agency);

        }

       

        #endregion


        #region Campaign By Agency

        public async Task<int> GetCountCampaignByAgency(int agencyid, CampaignType? type, CampaignStatus? status, string keyword)
        {
            var filter = new CampaignByAgencySpecification(agencyid, type, status, keyword);
            return await _campaignRepository.CountAsync(filter);
        }

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
                var payment = await _campaignRepository.GetCampaignPaymentByAgency(agencyid, id);
                return new CampaignDetailsViewModel(campaign, campaign.CampaignOption, campaign.CampaignAccount, payment, transactions);
            }
            return null;
        }

        public async Task<ListCampaignAccountViewModel> GetCampaignAccount(int campaignid, int page, int pagesize)
        {

            var filter = new CampaignAccountSpecification(campaignid);
            var total = await _campaignAccountRepository.CountAsync(filter);
            var campaingAccounts = await _campaignAccountRepository.ListPagedAsync(filter, "Id_desc", page, pagesize);

            return new ListCampaignAccountViewModel()
            {
                Pager = new PagerViewModel()
                {
                    Page = page,
                    PageSize = pagesize,
                    Total = total,
                },
                CampaignAccounts = CampaignAccountViewModel.GetList(campaingAccounts)
            };
        }

        public async Task<CampaignPaymentModel> GetCampaignPaymentByAgency(int agencyid, int id)
        {
            return await _campaignRepository.GetCampaignPaymentByAgency(agencyid, id);
        }


        public async Task<CreateCampaignInfoViewModel> GetCreateCampaign(int agencyid, CampaignType campaignType)
        {
            var code = await _campaignRepository.GetValidCode(agencyid);

            return new CreateCampaignInfoViewModel()
            {
                Code = code,
                Method = CampaignMethod.OpenJoined,
                Type = campaignType
            };
        }



        #region EditCampaignInfo

        public async Task<EditCampaignInfoViewModel> GetEditCampaignInfo(int agencyid, int id)
        {
            var filter = new CampaignByAgencySpecification(agencyid, id);
            var campaign = await _campaignRepository.GetSingleBySpecAsync(filter);
            if (campaign != null)
            {
                return new EditCampaignInfoViewModel(campaign);
            }
            return null;
        }
        public async Task<bool> EditCampaignInfo(EditCampaignInfoViewModel model, string username)
        {
            var campiagn = await _campaignRepository.GetByIdAsync(model.Id);
            if (campiagn == null || campiagn.Status != CampaignStatus.Created) return false;

            campiagn = model.GetEntity(campiagn);
            campiagn.UserModified = username;
            campiagn.DateModified = DateTime.Now;
            await _campaignRepository.UpdateAsync(campiagn);

            return true;
        }

        #endregion

        #region EditCampaignTarget
        public async Task<EditCampaignTargetViewModel> GetEditCampaignTarget(int agencyid, int id)
        {
            var filter = new CampaignByAgencySpecification(agencyid, id);
            var campaign = await _campaignRepository.GetSingleBySpecAsync(filter);
            if (campaign != null)
            {
                return new EditCampaignTargetViewModel(campaign);
            }
            return null;
        }
        public async Task<bool> EditCampaignTarget(EditCampaignTargetViewModel model, string username)
        {
            var campaign = await _campaignRepository.GetByIdAsync(model.Id);

            if (campaign == null || campaign.Status != CampaignStatus.Created) return false;

            campaign = model.GetEntity(campaign);

            campaign.UserModified = username;
            campaign.DateModified = DateTime.Now;





            await _campaignRepository.UpdateAsync(campaign);





            await CreateCampaignAccountType(campaign.Id, model.AccountType, username,true);
            await CreateCampaignOptions(campaign.Id, model, username, true);

            return true;
        }

        #endregion

        public async Task<int> CreateCampaign(int agencyid, CreateCampaignInfoViewModel info, CreateCampaignTargetViewModel target, string username)
        {
            var campaignTypeCharge = await _campaignTypeChargeRepository.GetSingleBySpecAsync(new CampaignTypeChargeSpecification(info.Type));
            if (campaignTypeCharge == null)
            {
                return -1;
            }
            var settings = await _settingRepository.GetSetting();
            var code = await _campaignRepository.GetValidCode(agencyid);
            var campaign = CreateCampaignViewModel.GetEntity(agencyid, info, target, campaignTypeCharge, settings, code, username);


            await _campaignRepository.AddAsync(campaign);
            if (campaign.Id > 0)
            {
                await CreateCampaignAccountType(campaign.Id, target.AccountType, username);
                await CreateCampaignOptions(campaign.Id, target, username);
                return campaign.Id;
            }

            return -1;
        }

        private async Task CreateCampaignAccountType(int campaignId, IEnumerable<AccountType> accountTypes, string username, bool hasRemove = false)
        {
            if (hasRemove)
            {
                var ls = await _campaignAccountTypeRepository.ListAsync(new CampaignAccountTypeByCampaignSpecification(campaignId));
                foreach(var item in ls)
                {
                    await _campaignAccountTypeRepository.DeleteAsync(item);
                }
            }
            foreach (var accountType in accountTypes)
            {

                await _campaignAccountTypeRepository.AddAsync(new CampaignAccountType()
                {
                    AccountType = accountType,
                    CampaignId = campaignId,

                });
            }
        }


        public async Task<int> GetAgencyChagreAmount(int campaignAccountId)
        {
            var campaignAccount = await _campaignAccountRepository.GetByIdAsync(campaignAccountId);
            if (campaignAccount != null)
            {
                var campaign = await _campaignRepository.GetByIdAsync(campaignAccount.CampaignId);
                if (campaign != null)
                {
                    return campaign.GetAgencyChagreAmount(campaignAccount);
                }
            }
            return 0;
        }

        private async Task CreateCampaignOptions(int campaignId, CreateCampaignTargetViewModel model, string username, bool hasRemove = false)
        {


            if (hasRemove)
            {
                var ls = await _campaignOptionRepository.ListAsync(new CampaignOptionByCampaignSpecification(campaignId));
                foreach (var item in ls)
                {
                    await _campaignOptionRepository.DeleteAsync(item);
                }
            }


            if (model.AccountType.Contains(AccountType.HotMom))
            {
                if (model.ChildType.HasValue)
                {
                    await _campaignOptionRepository.AddAsync(new CampaignOption()
                    {
                        CampaignId = campaignId,
                        Name = CampaignOptionName.Child,
                        Value = $"{model.ChildType}|{model.ChildAgeMin}-{model.ChildAgeMax}"
                    });
                }
            }

            if (model.EnabledCity && model.CityId != null && model.CityId.Count > 0)
            {

                foreach (var cityId in model.CityId)
                {
                    await _campaignOptionRepository.AddAsync(new CampaignOption()
                    {
                        CampaignId = campaignId,
                        Name = CampaignOptionName.City,
                        Value = cityId.ToString()
                    });
                }


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


        public async Task RemoveCampaignAccount(int campaignid)
        {
            var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountByAgencySpecification(campaignid));
            foreach (var item in campaignAccounts)
            {
                await _campaignAccountRepository.DeleteAsync(item);
            }


        }

        public async Task UpdateCampaignAccountExpired(int campaignid = 0, int agencyid = 0)
        {
            if (campaignid == 0)
            {
                var campaigns = await _campaignRepository.ListAsync(new CampaignExpiredFeedbackTimeSpecification());

                foreach (var campaign in campaigns)
                {
                    BackgroundJob.Enqueue<ICampaignService>(m => m.UpdateCampaignAccountExpired(campaign.Id, campaign.AgencyId));
                }
            }
            else
            {
                var campaign = await _campaignRepository.GetByIdAsync(campaignid);

                var username = "bot";
                var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountByAgencySpecification(campaignid, CampaignAccountStatus.AgencyRequest));

                foreach (var campaignAccount in campaignAccounts)
                {
                    campaignAccount.Status = CampaignAccountStatus.Canceled;
                    campaignAccount.DateModified = DateTime.Now;
                    campaignAccount.UserModified = username;
                    await _campaignAccountRepository.UpdateAsync(campaignAccount);

                    var notifType = NotificationType.AccountDeclineJoinCampaign;
                    await _notificationRepository.AddAsync(new Notification()
                    {
                        Type = notifType,
                        DataId = campaignid,
                        Data = string.Empty,
                        DateCreated = DateTime.Now,
                        EntityType = EntityType.Agency,
                        EntityId = agencyid,
                        Message = notifType.GetMessageText(username, campaign.Title.ToString()),
                        Status = NotificationStatus.Created
                    });

                }

            }


        }


        public async Task<CampaignAccountByAccountViewModel> GetCampaignAccountByAccount(int accountid, int campaignid)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            if (campaign != null)
            {


                var filter = new CampaignAccountByAccountSpecification(accountid, campaignid);
                var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(filter);

                if (campaignAccount != null)
                    return new CampaignAccountByAccountViewModel(campaignAccount, campaign);
            }
            return null;
        }

        public async Task<bool> CreateCampaignAccount(int agencyid, int campaignid, int accountid, int amount, string username)
        {
            var createStatus = await _campaignAccountRepository.CreateCampaignAccount(agencyid, campaignid, accountid, amount, username);

            return (createStatus > 0);
        }

        public async Task<bool> RequestJoinCampaignByAgency(int agencyid, int campaignid, string username)
        {
            var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountByAgencySpecification(campaignid, CampaignAccountStatus.WaitToPay), false);
            var campaign = await _campaignRepository.GetByIdAsync(campaignid);

            foreach (var campaignAccount in campaignAccounts)
            {
                campaignAccount.Status = CampaignAccountStatus.AgencyRequest;
                campaignAccount.DateModified = DateTime.Now;
                campaignAccount.UserModified = username;
                await _campaignAccountRepository.UpdateAsync(campaignAccount);

                await _notificationRepository.CreateNotification(NotificationType.AgencyRequestJoinCampaign, EntityType.Account, campaignAccount.AccountId, campaignid,
                    NotificationType.AgencyRequestJoinCampaign.GetMessageText(username, campaign.Title.ToString()));
            }


            return false;

        }

        public async Task<bool> RequestJoinCampaignByAccount(int accountid, RequestJoinCampaignViewModel model, string username)
        {
            var campaign = await _campaignRepository.GetByIdAsync(model.CampaignId);
            if (campaign != null)
            {

                if (campaign.Type == CampaignType.ShareContentWithCaption && string.IsNullOrEmpty(model.Caption))
                {
                    return false;
                }

                var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(new CampaignAccountByAccountSpecification(accountid, model.CampaignId));

                if (campaignAccount == null)
                {
                    campaignAccount = new CampaignAccount()
                    {
                        CampaignId = campaign.Id,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        AccountChargeAmount = model.AccountChargeAmount,
                        Note = string.Empty,
                        AccountId = accountid,
                        UserModified = username,
                        UserCreated = username,
                        Type = campaign.Type,
                        KPICommitted = model.KPICommitted,
                        Status = CampaignAccountStatus.AccountRequest,
                        ReviewAddress = model.ReviewAddress
                    };
                    await _campaignAccountRepository.AddAsync(campaignAccount);



                    if (campaign.Type == CampaignType.ShareContentWithCaption)
                    {
                        await _campaignAccountCaptionRepository.AddAsync(new CampaignAccountCaption()
                        {
                            CampaignAccountId = campaignAccount.Id,
                            Content = model.Caption,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            Status = CampaignAccountCaptionStatus.ChoDuyet,
                            UserCreated = username,
                            UserModified = username
                        });
                    }


                    await _notificationRepository.CreateNotification(NotificationType.AccountRequestJoinCampaign, EntityType.Agency, campaign.AgencyId, campaign.Id,
                        NotificationType.AccountRequestJoinCampaign.GetMessageText(username, campaign.Title.ToString()));

                    return true;

                }
            }
            return false;
        }

        public async Task<bool> RequestJoinCampaignByAgency(int agencyid, int campaignid, int accountid, string username)
        {

            var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(new CampaignAccountByAccountSpecification(accountid, campaignid));
            var campaign = await _campaignRepository.GetByIdAsync(campaignid);

            if (campaignAccount == null)
            {
                
                if (campaign == null)
                {
                    return false;
                }
                campaignAccount = new CampaignAccount()
                {
                    CampaignId = campaignid,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    AccountChargeAmount = campaign.AmountMax,
                    Note = string.Empty,
                    AccountId = accountid,
                    UserModified = username,
                    UserCreated = username,
                    Type = campaign.Type,
                    Status = CampaignAccountStatus.AgencyRequest,
                };
                await _campaignAccountRepository.AddAsync(campaignAccount);


            }
            else
            {

                

                campaignAccount.Status = CampaignAccountStatus.AgencyRequest;
                campaignAccount.DateModified = DateTime.Now;
                campaignAccount.UserModified = username;
                await _campaignAccountRepository.UpdateAsync(campaignAccount);

                await _notificationRepository.CreateNotification(NotificationType.AgencyRequestJoinCampaign, EntityType.Account, accountid, campaignid,
                    NotificationType.AgencyRequestJoinCampaign.GetMessageText(username, campaign.Title.ToString()));



            }

            await _notificationRepository.CreateNotification(NotificationType.AgencyRequestJoinCampaign, EntityType.Account, accountid, campaignid,
                NotificationType.AgencyRequestJoinCampaign.GetMessageText(username, campaign.Title.ToString()));

            return true;

        }

        public async Task<bool> FeedbackJoinCampaignByAccount(int accountid, RequestJoinCampaignViewModel model, string username, bool confirmed)
        {

            var campaign = await _campaignRepository.GetByIdAsync(model.CampaignId);
            if (campaign != null)
            {
                var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(new CampaignAccountByAccountSpecification(accountid, model.CampaignId));

                if (campaignAccount != null)
                {
                    if (campaignAccount.Status == CampaignAccountStatus.AgencyRequest)
                    {
                        campaignAccount.Status = confirmed ? CampaignAccountStatus.Confirmed : CampaignAccountStatus.Canceled;
                        campaignAccount.DateModified = DateTime.Now;
                        campaignAccount.UserModified = username;
                        campaignAccount.KPICommitted = model.KPICommitted;
                        if (confirmed)
                        {
                            campaignAccount.AccountChargeAmount = model.AccountChargeAmount;
                            campaignAccount.ReviewAddress = model.ReviewAddress;
                        }
                        else
                        {
                            campaignAccount.AccountChargeAmount = 0;

                        }
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
                            Message = notifType.GetMessageText(username, campaign.Title.ToString()),
                            Status = NotificationStatus.Created
                        });

                        return true;
                    }
                }
                else
                {
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
                            Message = notifType.GetMessageText(username, campaign.Title.ToString()),
                            Status = NotificationStatus.Created
                        });

                        return true;
                    }

                }
            }

            return false;
        }


        public async Task<CampaignViewModel> GetCampaignByRefId(int accountid, string facebookid)
        {
            var spec = new CampaignAccountByRefIdSpecification(facebookid);
            var accountCampaign = await _campaignAccountRepository.GetSingleBySpecAsync(spec);
            if (accountCampaign != null)
            {

                var campaign = await _campaignRepository.GetByIdAsync(accountCampaign.CampaignId);

                return new CampaignViewModel(campaign);
            }
            return null;
        }


        public async Task<bool> UpdateExecutionTime(int agencyid, int campaignid, string date, string username)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            if (campaign != null)
            {

                var executionTime = DateRangeHelper.GetDateRange(date);
                if (executionTime.HasValue)
                {
                    campaign.ExecutionStart = (DateTime?)executionTime.Value.Start;
                    campaign.ExecutionEnd = (DateTime?)executionTime.Value.End;
                    campaign.DateModified = DateTime.Now;
                    campaign.UserModified = username;
                    await _campaignRepository.UpdateAsync(campaign);
                    return true;
                }
            }
            return false;

        }
        #endregion


        #region CampaignCounterViewModel

        public async Task<CampaignCounterViewModel> GetCampaignCounterByAccount(int accountid)
        {
            var filter = new CampaignAccountByAccountSpecification(accountid, new List<CampaignAccountStatus>()
            {
                CampaignAccountStatus.Confirmed,
                CampaignAccountStatus.SubmittedContent,
                CampaignAccountStatus.DeclinedContent,
                CampaignAccountStatus.ApprovedContent,
                CampaignAccountStatus.UpdatedContent,
                CampaignAccountStatus.Finished,
            });
            var filter2 = new CampaignAccountByAccountSpecification(accountid, new List<CampaignAccountStatus>()
            {
                CampaignAccountStatus.SubmittedContent,
                CampaignAccountStatus.DeclinedContent,
                CampaignAccountStatus.ApprovedContent,
                CampaignAccountStatus.UpdatedContent,
            });

            var filter3 = new CampaignAccountByAccountSpecification(accountid, new List<CampaignAccountStatus>()
            {
                CampaignAccountStatus.Finished,
            });

            return new CampaignCounterViewModel()
            {
                Total = await _campaignAccountRepository.CountAsync(filter),
                TotalProcess = await _campaignAccountRepository.CountAsync(filter2),
                TotalFinished = await _campaignAccountRepository.CountAsync(filter3),
            };


        }
        #endregion

        #region Action


        public async Task UpdateCampaignServiceChargePercent(int ServiceChargePercent, int campaignid)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            campaign.ServiceChargePercent = ServiceChargePercent;
            await _campaignRepository.UpdateAsync(campaign);
        }

        public async Task<bool> ReportCampaignAccount(int agencyid, ReportCampaignAccountViewModel model, string username)
        {
            var campaignAccount = await _campaignAccountRepository.GetByIdAsync(model.Id);
            if (campaignAccount.ReportStatus.HasValue && campaignAccount != null)
            {
                if(campaignAccount.ReportStatus.Value != CampaignAccountReportStatus.Reported)
                {
                    campaignAccount.ReportStatus = CampaignAccountReportStatus.Reported;
                    campaignAccount.ReportNote = model.Note;
                    campaignAccount.ReportImages = model.Image;
                    campaignAccount.DateModified = DateTime.Now;
                    campaignAccount.UserModified = username;
                    await _campaignAccountRepository.UpdateAsync(campaignAccount);
                }
                
            }
            else
            {
                return false;
            }
            

           
            return true;
        }

        public async Task<bool> UpdateCampaignAccountRating(int agencyid, UpdateCampaignAccountRatingViewModel model, string username)
        {
            var campaignAccount = await _campaignAccountRepository.GetByIdAsync(model.Id);
            if (campaignAccount == null)
            {
                return false;
            }

            campaignAccount.Rating = model.Rating;
            campaignAccount.DateModified = DateTime.Now;
            campaignAccount.UserModified = username;
            await _campaignAccountRepository.UpdateAsync(campaignAccount);
            return true;
        }

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
                if (status == CampaignStatus.Canceled && campaign.Status != CampaignStatus.Created)
                {
                    return -1;
                }

                if (status == CampaignStatus.Started && campaign.Status != CampaignStatus.Confirmed)
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





                //if (status == CampaignStatus.Started)
                //{
                //    campaign.DateStart = DateTime.Now;
                //}
                //if (status == CampaignStatus.Ended)
                //{
                //    campaign.DateEnd = DateTime.Now;
                //}


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
                 NotificationType.AccountFinishCampaignRefContent.GetMessageText(username, campaign.Title.ToString()),
                 campaignAccount.Id.ToString());
            return 1;
        }



        public async Task<int> UpdateReviewAddress(int id, string addresss, string username)
        {
            var accountCampaign = await _campaignAccountRepository.GetByIdAsync(id);
            if (accountCampaign != null)
            {
                if (string.IsNullOrEmpty(accountCampaign.ReviewAddress))
                {
                    accountCampaign.ReviewAddress = addresss;
                    accountCampaign.DateModified = DateTime.Now;
                    accountCampaign.UserModified = username;
                    await _campaignAccountRepository.UpdateAsync(accountCampaign);
                    return accountCampaign.CampaignId;
                }
                return -2;
            }

            return -1;
        }

        public async Task<int> UpdateCampaignAccountStatus(int id, CampaignAccountStatus status, string msg)
        {
            var accountCampaign = await _campaignAccountRepository.GetByIdAsync(id);
            if (accountCampaign != null)
            {

                accountCampaign.Status = status;
                accountCampaign.Note = msg;
                accountCampaign.DateModified = DateTime.Now;
                await _campaignAccountRepository.UpdateAsync(accountCampaign);
                return accountCampaign.CampaignId;

                
            }

            return -1;
        }



        public async Task<int> UpdateCampaignAccountRef(int accountid, UpdateCampaignAccountRefViewModel model, string username)
        {
            var campaign = await _campaignRepository.GetByIdAsync(model.CampaignId);
            if (campaign == null) return -1;

            var filter = new CampaignAccountByAccountSpecification(accountid, campaign.Id);
            var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(filter);
            if (campaignAccount == null) return -1;



            if(campaign.Type != CampaignType.ChangeAvatar || campaign.Type  != CampaignType.ReviewProduct || campaign.Type != CampaignType.JoinEvent)
            {
                if (campaign.Data.Contains("https") || campaign.Data.Contains("http"))
                {
                    if (!model.RefUrl.Contains(campaign.Data))
                    {
                        return -1;
                    }
                }
            }

            



            if (!string.IsNullOrEmpty(model.RefId))
            {
                campaignAccount.RefId = model.RefId;
            }
            if (!string.IsNullOrEmpty(model.RefUrl))
            {
                campaignAccount.RefUrl = model.RefUrl;
            }



            campaignAccount.RefImage = model.RefImage.ToListString();
            campaignAccount.Status = CampaignAccountStatus.Finished;
            campaignAccount.DateModified = DateTime.Now;
            campaignAccount.UserModified = username;
            await _campaignAccountRepository.UpdateAsync(campaignAccount);

            //notification

            await _notificationRepository.CreateNotification(NotificationType.AccountFinishCampaignRefContent,
                EntityType.Agency, campaign.AgencyId, campaign.Id,
                 NotificationType.AccountFinishCampaignRefContent.GetMessageText(username, campaign.Title.ToString()),
                 campaignAccount.Id.ToString());
            return campaignAccount.Id;
        }

        public async Task<int> UpdateCampaignAccountRefImages(int accountid, UpdateCampaignAccountRefImagesViewModel model, string username)
        {
            var campaign = await _campaignRepository.GetByIdAsync(model.CampaignId);
            if (campaign == null) return -1;

            var filter = new CampaignAccountByAccountSpecification(accountid, campaign.Id);
            var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(filter);
            if (campaignAccount == null) return -1;


            campaignAccount.RefImage = model.RefImage.ToListString();
            campaignAccount.DateModified = DateTime.Now;
            campaignAccount.UserModified = username;
            await _campaignAccountRepository.UpdateAsync(campaignAccount);

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

            campaignAccount.Status = CampaignAccountStatus.SubmittedContent;
            campaignAccount.DateModified = DateTime.Now;
            campaignAccount.UserModified = username;
            await _campaignAccountRepository.UpdateAsync(campaignAccount);

            //notification

            await _notificationRepository.CreateNotification(NotificationType.AccountSubmitCampaignRefContent,
                EntityType.Agency, campaign.AgencyId, campaign.Id,
                 NotificationType.AccountSubmitCampaignRefContent.GetMessageText(username, campaign.Title.ToString()),
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


            var status = type == 1 ? CampaignAccountStatus.ApprovedContent :
                type == 2 ? CampaignAccountStatus.UpdatedContent : CampaignAccountStatus.DeclinedContent;
            campaignAccount.Status = status;
            if (status == CampaignAccountStatus.UpdatedContent)
            {
                campaignAccount.RefContent = newContent;
            }
            campaignAccount.DateModified = DateTime.Now;
            campaignAccount.UserModified = username;
            await _campaignAccountRepository.UpdateAsync(campaignAccount);

            //notification
            var notiftype = type == 1 ? NotificationType.AgencyApproveCampaignRefContent : NotificationType.AgencyDeclineCampaignRefContent;
            await _notificationRepository.CreateNotification(notiftype,
                EntityType.Account, campaignAccount.AccountId, campaign.Id,
                 notiftype.GetMessageText(username, campaign.Title.ToString()),
                 campaignAccount.Id.ToString());

            return 1;
        }

        public async Task<int> CancelCampaignAccount(int campaignid, int accountid, string note, string username)
        {

            var filter = new CampaignAccountByAccountSpecification(accountid, campaignid);
            var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(filter);
            if (campaignAccount == null) return -1;


            campaignAccount.Status = CampaignAccountStatus.Canceled;
            campaignAccount.DateModified = DateTime.Now;
            campaignAccount.UserModified = username;
            await _campaignAccountRepository.UpdateAsync(campaignAccount);

            //notification
            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            await _notificationRepository.CreateNotification(NotificationType.AgencyCancelAccountJoinCampaign,
                EntityType.Account, campaignAccount.AccountId, campaignid,
                 NotificationType.AgencyCancelAccountJoinCampaign.GetMessageText(username, campaign.Title.ToString()),
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

        #region Auto Update campaign Status

        public async Task AutoUpdateStartedStatus(int campaignid)
        {
            if (campaignid == 0)
            {
                var campaignids = await _campaignRepository.GetCampaignIdNeedToStart();

                foreach (var id in campaignids)
                {
                    BackgroundJob.Enqueue<ICampaignService>(m => m.AutoUpdateStartedStatus(id));
                }
            }
            else
            {
                var username = "system";
                var campaign = await _campaignRepository.GetByIdAsync(campaignid);             
                if (campaign != null && campaign.Status == CampaignStatus.Confirmed)
                {

                    //check them thời gian nhận đăng ký thì mới quyết định start
                    //ví dụ trường hợp này sẽ lỗi làm cho chiến dịch bị cancel trong khi chưa hết thời gian chạy chiến dịch
                    //thời gian nhận đăng ký và chạy chiến dịch quá gần nhau sẽ bị trường hợp này -> cancel đột ngột.
                    /* Ví dụ
                     Thời gian nhận đăng ký: 01:54 09/04/2020 - 18:00 09/04/2020
                     Thời gian thực hiện: 01:54 09/04/2020 - 11:00 10/04/2020
                     */
                    DateTime now = DateTime.Now;
                    if (campaign.DateStart <= now && campaign.DateEnd >= now)
                        return;
                    //------------------------------------------------------------------------------------------------------------

                    #region check thanh toán đủ chưa
                    //check da thanh toan chua                    
                    // trường hợp thanh toán chưa đủ thì campaign sẽ bị khóa 
                    var payment = await _campaignRepository.GetCampaignPaymentByAgency(campaign.AgencyId, campaign.Id);
                    if (payment == null || !payment.IsValidToProcess)
                    {

                        campaign.Status = CampaignStatus.Locked; // locked chiến dịch
                        campaign.SystemNote = "Chưa thanh toán";
                        campaign.UserModified = username;
                        campaign.DateModified = DateTime.Now;
                        await _campaignRepository.UpdateAsync(campaign);

                        //await _notificationRepository.CreateNotification(NotificationType.CampaignCantStarted,
                        //   EntityType.Agency, campaign.AgencyId, campaign.Id,
                        //   NotificationType.CampaignCantStarted.GetMessageText("Hệ thống", campaign.Code, "Bạn chưa thanh toán tiền chiến dịch, hãy thanh toán để thực hiện được chiến dịch."));

                        // longhk edit
                        await _notificationRepository.CreateNotification(campaign.Id, EntityType.Agency, campaign.AgencyId, NotificationType.CampaignCantStarted, 
                            $"Để thực hiện được chiến dịch {campaign.Title}, bạn cần thanh toán {payment.TotalChargeValue.ToPriceText()} cho chiến dịch.");
                        //#################################################################################################################################

                        //################ anh Long add them notification gửi về admin ####################################################################
                        try
                        {
                            string _msg = string.Format("Chiến dịch \"{0}\" của doanh nghiệp \"{1}\", chưa thanh toán số tiền {2}. Hệ thống đã khóa", campaign.Title, campaign.UserCreated, payment.TotalChargeValue.ToPriceText());
                            string _data = "Campaign";
                            await _notificationRepository.CreateNotification(campaign.Id, EntityType.System, 0, NotificationType.CampaignLocked, _msg, _data);
                        }
                        catch// tranh loi lam crash 
                        {}
                        //#################################################################################################################################
                        return;
                    }
                    //#####################################################################################################################################
                    #endregion


                    #region kiểm tra xem có người nào tham gia không
                    var countCampaignAccountToProcess = 
                        await _campaignAccountRepository.CountAsync(new CampaignAccountSpecification(campaignid, null, 
                           new List<CampaignAccountStatus> { //condition ignore status by list status
                            CampaignAccountStatus.AccountRequest, //ignore status
                            CampaignAccountStatus.AgencyRequest, //ignore status
                            CampaignAccountStatus.Canceled, //ignore status
                            CampaignAccountStatus.WaitToPay, //ignore status

                    }));

                    // neu ko co ai thuc hien thi huy ko start nua
                    if (countCampaignAccountToProcess == 0)
                    {
                        campaign.Status = CampaignStatus.Canceled;
                        campaign.SystemNote = "Không có thành viên thực hiện chiến dịch";
                        campaign.UserModified = username;
                        campaign.DateModified = DateTime.Now;
                        await _campaignRepository.UpdateAsync(campaign);

                        await _notificationRepository.CreateNotification(NotificationType.CampaignCanceled,
                            EntityType.Agency, campaign.AgencyId, campaign.Id,
                            NotificationType.CampaignCanceled.GetMessageText(campaign.Code, "Không có thành viên thực hiện chiến dịch"));

                        //################ anh Long add them notification gửi về admin ####################################################################
                        try
                        {
                            string _msg = string.Format("Chiến dịch \"{0}\" của doanh nghiệp \"{1}\", đã bị hủy. Không có thành viên thực hiện chiến dịch", campaign.Title, campaign.UserCreated);
                            string _data = "Campaign";
                            await _notificationRepository.CreateNotification(campaign.Id, EntityType.System, 0, NotificationType.CampaignCanceled, _msg, _data);
                        }
                        catch// tranh loi lam crash 
                        { }
                        return;
                    }
                    #endregion


                    // huy cac thanh vien chua co caption - content voi loai checkin - review
                    campaign.Status = CampaignStatus.Started;
                    campaign.UserModified = username;
                    campaign.DateModified = DateTime.Now;
                    await _campaignRepository.UpdateAsync(campaign);
                    BackgroundJob.Enqueue<INotificationService>(m => m.CreateNotificationCampaignStarted(campaignid));

                    await CampaignStartedNotifyToAccount(campaign); // chien dịch started thi gưi thống báo đến tất cả người đã đồng ý tham gia chiến dịch
                    //################ anh Long add them notification gửi về admin ####################################################################
                    try
                    {
                        string _msg = string.Format("Chiến dịch \"{0}\" của doanh nghiệp \"{1}\", Đã bắt đầu", campaign.Title, campaign.UserCreated);
                        string _data = "Campaign";
                        await _notificationRepository.CreateNotification(campaign.Id, EntityType.System, 0, NotificationType.CampaignStarted, _msg, _data);
                    }
                    catch// tranh loi lam crash 
                    { }

                    // longhk thêm gửi notification đến doanh nghiệp khi chiến dịch bắt đầu
                    await _notificationRepository.CreateNotification(campaign.Id, EntityType.Agency, campaign.AgencyId, NotificationType.CampaignStarted,
                        $"Chiến dịch {campaign.Title}, của bạn đã bắt đầu.");
                    //#################################################################################################################################



                    //huy campaing account chua confirms 
                    var needCancelCampaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaignid, new List<CampaignAccountStatus> {
                            CampaignAccountStatus.AccountRequest,
                            CampaignAccountStatus.AgencyRequest,
                    }, null));

                    foreach (var campaignAccount in needCancelCampaignAccounts)
                    {
                        campaignAccount.Status = CampaignAccountStatus.Canceled;
                        campaignAccount.DateModified = DateTime.Now;
                        campaignAccount.UserModified = username;
                        await _campaignAccountRepository.UpdateAsync(campaignAccount);

                        //notification
                        await _notificationRepository.CreateNotification(NotificationType.AgencyCancelAccountJoinCampaign,
                            EntityType.Account, campaignAccount.AccountId, campaignid,
                             NotificationType.AgencyCancelAccountJoinCampaign.GetMessageText("Hệ thống", campaign.Code),
                             campaignAccount.Id.ToString());                      
                    }
                }
            }

        }

        private async Task CampaignStartedNotifyToAccount(Campaign campaign)
        {
            //CampaignAccountStatus.Confirmed
            try
            {
                // chỉ lấy account đã confirm tham gia chiến dịch và gửi thông báo đến account
                var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaign.Id, CampaignAccountStatus.Confirmed));
                foreach(var account in campaignAccounts)
                {
                    //send notification to account
                    await _notificationRepository.CreateNotification(NotificationType.CampaignStarted,
                        EntityType.Account, account.AccountId, campaign.Id, string.Format("Đã đến thời gian thực hiện chiến dịch \"{0}\", bạn hãy thực hiện chiến dịch nhé!", campaign.Title),
                         account.Id.ToString());
                }

            }
            catch { }
        }



        //##################################### addition by longhk ##############################################################################
        public async Task RunCheckingLockedStatus(int campaignid)
        {
            if (campaignid == 0)
            {
                var campaignids = await this.GetLockedCampaignIds();

                foreach (var id in campaignids)
                {

                    BackgroundJob.Enqueue<ICampaignService>(m => m.RunCheckingLockedStatus(id));
                }
            }
            else
            {


                try {
                    var username = "system";
                    var campaign = await _campaignRepository.GetByIdAsync(campaignid);
                    if (campaign != null)
                    {
                        if (campaign.Status == CampaignStatus.Locked)
                        {
                            /*Kiểm tra thời gian thực hiện chiến dịch kết thúc có bé hơn ngày giờ hiện tại hay không để hủy bỏ chiến dịch*/
                            if (campaign.ExecutionEnd.HasValue)
                            {
                                if (campaign.ExecutionEnd.Value <= DateTime.Now)
                                {

                                    campaign.Status = CampaignStatus.Canceled;
                                    campaign.UserModified = username;
                                    campaign.DateModified = DateTime.Now;
                                    await _campaignRepository.UpdateAsync(campaign);

                                    /*Kiểm tra xem đã thanh toán chiến dịch lần nào chưa, nếu rồi gửi thông báo tạo lệnh rút tiền*/
                                    string _message = string.Format("Chiến dịch {0} đã bị hủy, vì thời gian thực hiện đã hết và chưa thanh toán.", campaign.Title);
                                    var payment = await _campaignRepository.GetCampaignPaymentByAgency(campaign.AgencyId, campaign.Id);
                                    if (payment != null)
                                    {
                                        if (payment.TotalPaidAmount > 0)
                                        {
                                            _message += string.Format("Hãy tạo lệnh rút {0} từ chiến dịch", payment.TotalPaidAmount.ToPriceText());
                                        }
                                    }
                                    await _notificationRepository.CreateNotification(NotificationType.CampaignCanceled,
                                    EntityType.Agency, campaign.AgencyId, campaign.Id, _message);
                                    try
                                    {
                                        string _msg = string.Format("Chiến dịch \"{0}\" của doanh nghiệp \"{1}\", đã bị hủy. vì thời gian thực hiện đã hết và chưa thanh toán", campaign.Title, campaign.UserCreated);
                                        string _data = "Campaign";
                                        await _notificationRepository.CreateNotification(campaign.Id, EntityType.System, 0, NotificationType.CampaignCanceled, _msg, _data);
                                    }
                                    catch// tranh loi lam crash 
                                    { }
                                }
                            }
                        }
                    }
                }
                catch { }
                
                //return;
            }
        }
        //######################################################################################################################################

        public async Task AutoUpdateEndedStatus(int campaignid)
        {
            if (campaignid == 0)
            {
                var campaignids = await _campaignRepository.GetCampaignIdNeedToEnd();

                foreach (var id in campaignids)
                {
                    BackgroundJob.Enqueue<ICampaignService>(m => m.AutoUpdateEndedStatus(id));


                }
            }
            else
            {
                var username = "system";
                var campaign = await _campaignRepository.GetByIdAsync(campaignid);
                if (campaign != null)
                {
                    if (campaign.Status == CampaignStatus.Started)
                    {
                        campaign.Status = CampaignStatus.Ended;
                        campaign.UserModified = username;
                        campaign.DateModified = DateTime.Now;
                        await _campaignRepository.UpdateAsync(campaign);


                        BackgroundJob.Enqueue<INotificationService>(m => m.CreateNotificationCampaignEnded(campaignid));

                        //################ anh Long add them notification gửi về admin ####################################################################
                        try
                        {
                            string _msg = string.Format("Chiến dịch \"{0}\" của doanh nghiệp \"{1}\", Đã kết thúc", campaign.Title, campaign.UserCreated);
                            string _data = "Campaign";
                            await _notificationRepository.CreateNotification(campaign.Id, EntityType.System, 0, NotificationType.CampaignEnded, _msg, _data);
                        }
                        catch// tranh loi lam crash 
                        { }
                        // longhk thêm notification gửi đến agency
                        await _notificationRepository.CreateNotification(campaign.Id, EntityType.Agency, campaign.AgencyId, NotificationType.CampaignEnded,
                            $"Chiến dịch {campaign.Title}, của bạn đã kết thúc.");
                        //#################################################################################################################################



                        //huy campaing account chua hoan thanh 
                        var needUnFinishedCampaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaignid, null
                            , new List<CampaignAccountStatus> {
                            CampaignAccountStatus.Finished,
                            CampaignAccountStatus.Canceled,
                            CampaignAccountStatus.Unfinished
                            }));

                        foreach (var campaignAccount in needUnFinishedCampaignAccounts)
                        {
                            campaignAccount.Status = CampaignAccountStatus.Unfinished;
                            campaignAccount.DateModified = DateTime.Now;
                            campaignAccount.UserModified = username;
                            await _campaignAccountRepository.UpdateAsync(campaignAccount);

                            //notification
                            await _notificationRepository.CreateNotification(NotificationType.SystemUpdateUnfinishedAccountCampaign,
                                EntityType.Account, campaignAccount.AccountId, campaignid,
                                 NotificationType.SystemUpdateUnfinishedAccountCampaign.GetMessageText(campaign.Code),
                                 campaignAccount.Id.ToString());
                        }
                    }
                    else if (campaign.Status == CampaignStatus.Confirmed)
                    {
                        campaign.Status = CampaignStatus.Canceled;
                        campaign.SystemNote = "Hết thời gian thực hiện";
                        campaign.UserModified = username;
                        campaign.DateModified = DateTime.Now;
                        await _campaignRepository.UpdateAsync(campaign);

                        await _notificationRepository.CreateNotification(NotificationType.CampaignCanceled,
                            EntityType.Agency, campaign.AgencyId, campaign.Id,
                            NotificationType.CampaignCanceled.GetMessageText(campaign.Code, "Hết thời gian thực hiện"));

                        try
                        {
                            string _msg = string.Format("Chiến dịch \"{0}\" của doanh nghiệp \"{1}\", Đã kết thúc, Hết thời gian thực hiện", campaign.Title, campaign.UserCreated);
                            string _data = "Campaign";
                            await _notificationRepository.CreateNotification(campaign.Id, EntityType.System, 0, NotificationType.CampaignCanceled, _msg, _data);
                        }
                        catch// tranh loi lam crash 
                        { }



                        var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaignid, null, null));

                        foreach (var campaignAccount in campaignAccounts)
                        {
                            campaignAccount.Status = CampaignAccountStatus.Canceled;
                            campaignAccount.DateModified = DateTime.Now;
                            campaignAccount.UserModified = username;
                            await _campaignAccountRepository.UpdateAsync(campaignAccount);

                            //notification
                            await _notificationRepository.CreateNotification(NotificationType.SystemUpdateCanceledAccountCampaign,
                                EntityType.Account, campaignAccount.AccountId, campaignid,
                                 NotificationType.SystemUpdateCanceledAccountCampaign.GetMessageText(campaign.Code, "Hết thời gian thực hiện"));

                        }

                    }



                }
            }

        }

        #endregion
    }
}
