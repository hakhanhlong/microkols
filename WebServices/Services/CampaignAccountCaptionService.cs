
using Common.Extensions;
using Core.Entities;
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
    public class CampaignAccountCaptionService : BaseService, ICampaignAccountCaptionService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IAsyncRepository<CampaignAccountCaption> _CampaignAccountCaptionRepository;
        private readonly IAsyncRepository<CampaignAccount> _campaignAccountRepository;
        private readonly INotificationRepository _notificationRepository;
        public CampaignAccountCaptionService(IAsyncRepository<CampaignAccountCaption> CampaignAccountCaptionRepository,
            ICampaignRepository campaignRepository,
             IAsyncRepository<CampaignAccount> campaignAccountRepository,
            INotificationRepository notificationRepository)
        {
            _CampaignAccountCaptionRepository = CampaignAccountCaptionRepository;
            _notificationRepository = notificationRepository;
            _campaignRepository = campaignRepository;
            _campaignAccountRepository = campaignAccountRepository;
        }

        #region CampaignAccountCaption

        public async Task<bool> IsValidCaption(int campaignAccountid)
        {
            var spec = new CampaignAccountCaptionByCampaignAccountIdSpecification(campaignAccountid, CampaignAccountCaptionStatus.DaDuyet);
            var entity = await _CampaignAccountCaptionRepository.GetSingleBySpecAsync(spec);

            return entity != null;

        }
        public async Task<ListCampaignAccountCaptionViewModel> GetCampaignAccountCaptions(int campaignAccountId, string order, int page, int pagesize)
        {


            var filter = new CampaignAccountCaptionByCampaignAccountIdSpecification(campaignAccountId);

            var total = await _CampaignAccountCaptionRepository.CountAsync(filter);
            var list = await _CampaignAccountCaptionRepository.ListPagedAsync(filter, "DateModified_desc", page, pagesize);

            return new ListCampaignAccountCaptionViewModel()
            {
                CampaignAccountCaptions = CampaignAccountCaptionViewModel.GetList(list),
                Pager = new PagerViewModel()
                {
                    Page = page,
                    PageSize = pagesize,
                    Total = total
                }
            };
        }
        public async Task<ListGroupCampaignAccountCaptionViewModel> GetGroupCampaignAccountCaptionsByCampaignId(int campaignId, string order, int page, int pagesize)
        {


            var filter = new CampaignAccountCaptionByCampaignIdSpecification(campaignId);

            var query = _CampaignAccountCaptionRepository.GetQueryBySpecification(filter);

            var queryCampaignAccounts  = query.Select(m=> m.CampaignAccountId).Distinct();

            var total = await queryCampaignAccounts.CountAsync();
            var ids = await queryCampaignAccounts.OrderByDescending(m => m).GetPagedAsync(page, pagesize);

            var list = await _CampaignAccountCaptionRepository.ListAsync(new CampaignAccountCaptionByCampaignAccountIdSpecification(ids));

            return new ListGroupCampaignAccountCaptionViewModel(list, page, pagesize, total);
        }


        public async Task<int> CreateCampaignAccountCaption(CreateCampaignAccountCaptionViewModel model, string username)
        {
            var campaign = await _campaignRepository.GetByIdAsync(model.CampaignId);
            if (campaign == null) return -1;
            var entity = new CampaignAccountCaption()
            {
                CampaignAccountId = model.CampaignAccountId,
                Content = model.Content,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Note = string.Empty,
                Status = CampaignAccountCaptionStatus.ChoDuyet,
                UserCreated = username,
                UserModified = username,
            };
            await _CampaignAccountCaptionRepository.AddAsync(entity);

            var notifType = NotificationType.AccountSubmitCampaignCaption;
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
            return entity.Id;
        }
        public async Task<EditCampaignAccountCaptionViewModel> GetEditCampaignAccountCaption(int CampaignAccountCaptionId)
        {
            var filer = new CampaignAccountCaptionSpecification(CampaignAccountCaptionId);

            var CampaignAccountCaption = await _CampaignAccountCaptionRepository.GetSingleBySpecAsync(filer);

            if (CampaignAccountCaption == null)
                return null;

            return new EditCampaignAccountCaptionViewModel(CampaignAccountCaption);

        }

        public async Task<bool> EditCampaignAccountCaption(EditCampaignAccountCaptionViewModel model, string username)
        {
            var CampaignAccountCaption = await _CampaignAccountCaptionRepository.GetByIdAsync(model.Id);

            if (CampaignAccountCaption == null)
            {
                return false;
            }


            CampaignAccountCaption.Note = model.Note;
            CampaignAccountCaption.Content = model.Content;
            CampaignAccountCaption.DateModified = DateTime.Now;
            CampaignAccountCaption.UserModified = username;

            await _CampaignAccountCaptionRepository.UpdateAsync(CampaignAccountCaption);
            return true;
        }

        public async Task<bool> UpdateStatus(int id, CampaignAccountCaptionStatus status, string username)
        {
            var CampaignAccountCaption = await _CampaignAccountCaptionRepository.GetByIdAsync(id);
            if (CampaignAccountCaption == null)
            {
                return false;
            };

            var campaignaccount = await _campaignAccountRepository.GetByIdAsync(CampaignAccountCaption.CampaignAccountId);
            if (campaignaccount == null)
            {
                return false;
            };






            CampaignAccountCaption.Status = status;
            CampaignAccountCaption.UserModified = username;
            CampaignAccountCaption.DateModified = DateTime.Now;
            await _CampaignAccountCaptionRepository.UpdateAsync(CampaignAccountCaption);

            if (status == CampaignAccountCaptionStatus.DaDuyet)
            {
                campaignaccount.RefContent = CampaignAccountCaption.Content;

                campaignaccount.UserModified = username;

                campaignaccount.DateModified = DateTime.Now;

                campaignaccount.Status = CampaignAccountStatus.ApprovedContent;

                campaignaccount.IsApprovedContent = true;

                await _campaignAccountRepository.UpdateAsync(campaignaccount);

            }
            else if(status == CampaignAccountCaptionStatus.KhongDuyet)
            {
                campaignaccount.UserModified = username;
                campaignaccount.DateModified = DateTime.Now;
                campaignaccount.Status = CampaignAccountStatus.Canceled;
                await _campaignAccountRepository.UpdateAsync(campaignaccount);
            }

            var notifType = status== CampaignAccountCaptionStatus.DaDuyet ? NotificationType.AgencyApproveCampaignCaption : NotificationType.AgencyDeclineCampaignCaption;
            var campaign = await _campaignRepository.GetByIdAsync(campaignaccount.CampaignId);
            await _notificationRepository.AddAsync(new Notification()
            {
                Type = notifType,
                DataId = campaignaccount.CampaignId,
                Data = string.Empty,
                DateCreated = DateTime.Now,
                EntityType = EntityType.Account,
                EntityId = campaignaccount.AccountId,
                Message = notifType.GetMessageText(username, campaign.Title.ToString()),
                Status = NotificationStatus.Created
            });
            return true;
        }


        public async Task<bool> UpdateNote(int id, string note, string username)
        {
            var CampaignAccountCaption = await _CampaignAccountCaptionRepository.GetByIdAsync(id);
            if (CampaignAccountCaption == null)
            {
                return false;
            };

            var campaignaccount = await _campaignAccountRepository.GetByIdAsync(CampaignAccountCaption.CampaignAccountId);
            if (campaignaccount == null)
            {
                return false;
            };


            CampaignAccountCaption.Status = CampaignAccountCaptionStatus.YeuCauSua;

            CampaignAccountCaption.Note = note;
            CampaignAccountCaption.UserModified = username;
            CampaignAccountCaption.DateModified = DateTime.Now;
            await _CampaignAccountCaptionRepository.UpdateAsync(CampaignAccountCaption);

            var notifType =  NotificationType.AgencyUpdatedCampaignCaption ;
            var campaign = await _campaignRepository.GetByIdAsync(campaignaccount.CampaignId);
            await _notificationRepository.AddAsync(new Notification()
            {
                Type = notifType,
                DataId = campaignaccount.CampaignId,
                Data = string.Empty,
                DateCreated = DateTime.Now,
                EntityType = EntityType.Account,
                EntityId = campaignaccount.AccountId,
                Message = notifType.GetMessageText(username, campaign.Title.ToString()),
                Status = NotificationStatus.Created
            });
            return true;
        }


        
        #endregion
    }
}
