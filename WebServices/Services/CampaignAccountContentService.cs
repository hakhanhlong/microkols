
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
    public class CampaignAccountContentService : BaseService, ICampaignAccountContentService
    {
        private readonly IAsyncRepository<CampaignAccountContent> _CampaignAccountContentRepository;

        private readonly ICampaignRepository _campaignRepository;
        private readonly IAsyncRepository<CampaignAccount> _campaignAccountRepository;
        private readonly INotificationRepository _notificationRepository;
        public CampaignAccountContentService(IAsyncRepository<CampaignAccountContent> CampaignAccountContentRepository,
            ICampaignRepository campaignRepository,
             IAsyncRepository<CampaignAccount> campaignAccountRepository,
            INotificationRepository notificationRepository)
        {
            _CampaignAccountContentRepository = CampaignAccountContentRepository;
            _notificationRepository = notificationRepository;
            _campaignRepository = campaignRepository;
            _campaignAccountRepository = campaignAccountRepository;
        }

        #region CampaignAccountContent
        public async Task<ListCampaignAccountContentViewModel> GetCampaignAccountContents(int campaignAccountId, string order, int page, int pagesize)
        {


            var filter = new CampaignAccountContentByCampaignAccountIdSpecification(campaignAccountId);

            var total = await _CampaignAccountContentRepository.CountAsync(filter);
            var list = await _CampaignAccountContentRepository.ListPagedAsync(filter, "DateModified_desc", page, pagesize);

            return new ListCampaignAccountContentViewModel()
            {
                CampaignAccountContents = CampaignAccountContentViewModel.GetList(list),
                Pager = new PagerViewModel()
                {
                    Page = page,
                    PageSize = pagesize,
                    Total = total
                }
            };
        }

        public async Task<ListGroupCampaignAccountContentViewModel> GetGroupCampaignAccountContentsByCampaignId(int campaignId, string order, int page, int pagesize)
        {


            var filter = new CampaignAccountContentByCampaignIdSpecification(campaignId);
            var query = _CampaignAccountContentRepository.GetQueryBySpecification(filter);

            var queryCampaignAccounts = query.Select(m => m.CampaignAccountId).Distinct();
            var total = await queryCampaignAccounts.CountAsync();
            var ids = await queryCampaignAccounts.OrderByDescending(m => m).GetPagedAsync(page, pagesize);

            var list = await _CampaignAccountContentRepository.ListAsync(new CampaignAccountContentByCampaignAccountIdSpecification(ids));

            return new ListGroupCampaignAccountContentViewModel(list, page, pagesize, total);
        }


        public async Task<ListCampaignAccountContentViewModel> GetCampaignAccountContentsByCampaignId(int campaignid, string order, int page, int pagesize)
        {


            var filter = new CampaignAccountContentByCampaignIdSpecification(campaignid);

            var total = await _CampaignAccountContentRepository.CountAsync(filter);
            var list = await _CampaignAccountContentRepository.ListPagedAsync(filter, "DateModified_desc", page, pagesize);

            return new ListCampaignAccountContentViewModel()
            {
                CampaignAccountContents = CampaignAccountContentViewModel.GetList(list),
                Pager = new PagerViewModel()
                {
                    Page = page,
                    PageSize = pagesize,
                    Total = total
                }
            };
        }



        public async Task<int> CreateCampaignAccountContent(CreateCampaignAccountContentViewModel model, string username)
        {
            var campaign = await _campaignRepository.GetByIdAsync(model.CampaignId);
            if (campaign == null) return -1;

            var entity = new CampaignAccountContent()
            {
                CampaignAccountId = model.CampaignAccountId,
                Content = model.Content,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Note = string.Empty,
                Status = CampaignAccountContentStatus.ChoDuyet,
                UserCreated = username,
                UserModified = username,
                Image = model.Image.ToListString()
            };
            await _CampaignAccountContentRepository.AddAsync(entity);

            var notifType = NotificationType.AccountSubmitCampaignContent;
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

            return entity.Id;
        }
        public async Task<EditCampaignAccountContentViewModel> GetEditCampaignAccountContent(int CampaignAccountContentId)
        {
            var filer = new CampaignAccountContentSpecification(CampaignAccountContentId);

            var CampaignAccountContent = await _CampaignAccountContentRepository.GetSingleBySpecAsync(filer);

            if (CampaignAccountContent == null)
                return null;

            return new EditCampaignAccountContentViewModel(CampaignAccountContent);

        }

        public async Task<bool> EditCampaignAccountContent(EditCampaignAccountContentViewModel model, string username)
        {
            var CampaignAccountContent = await _CampaignAccountContentRepository.GetByIdAsync(model.Id);

            if (CampaignAccountContent == null)
            {
                return false;
            }
            CampaignAccountContent.Note = model.Note;
            CampaignAccountContent.Content = model.Content;
            CampaignAccountContent.DateModified = DateTime.Now;
            CampaignAccountContent.UserModified = username;

            await _CampaignAccountContentRepository.UpdateAsync(CampaignAccountContent);
            return true;
        }

        public async Task<bool> UpdateStatus(int id, CampaignAccountContentStatus status, string username)
        {
            var CampaignAccountContent = await _CampaignAccountContentRepository.GetByIdAsync(id);
            if (CampaignAccountContent == null)
            {
                return false;
            };
            var campaignaccount = await _campaignAccountRepository.GetByIdAsync(CampaignAccountContent.CampaignAccountId);
            if (campaignaccount == null)
            {
                return false;
            };


            CampaignAccountContent.Status = status;
            CampaignAccountContent.UserModified = username;
            CampaignAccountContent.DateModified = DateTime.Now;
            await _CampaignAccountContentRepository.UpdateAsync(CampaignAccountContent);

            var notifType = status == CampaignAccountContentStatus.DaDuyet ? NotificationType.AgencyApproveCampaignContent : NotificationType.AgencyDeclineCampaignContent;
            await _notificationRepository.AddAsync(new Notification()
            {
                Type = notifType,
                DataId = campaignaccount.CampaignId,
                Data = string.Empty,
                DateCreated = DateTime.Now,
                EntityType = EntityType.Account,
                EntityId = campaignaccount.AccountId,
                Message = notifType.GetMessageText(username, campaignaccount.CampaignId.ToString()),
                Status = NotificationStatus.Created
            });
            return true;
        }


        public async Task<bool> UpdateNote(int id, string note, string username)
        {
            var CampaignAccountContent = await _CampaignAccountContentRepository.GetByIdAsync(id);
            if (CampaignAccountContent == null)
            {
                return false;
            };

            var campaignaccount = await _campaignAccountRepository.GetByIdAsync(CampaignAccountContent.CampaignAccountId);
            if (campaignaccount == null)
            {
                return false;
            };



            CampaignAccountContent.Note = note;
            CampaignAccountContent.UserModified = username;
            CampaignAccountContent.DateModified = DateTime.Now;
            await _CampaignAccountContentRepository.UpdateAsync(CampaignAccountContent);

            var notifType = NotificationType.AgencyUpdatedCampaignContent;
            await _notificationRepository.AddAsync(new Notification()
            {
                Type = notifType,
                DataId = campaignaccount.CampaignId,
                Data = string.Empty,
                DateCreated = DateTime.Now,
                EntityType = EntityType.Account,
                EntityId = campaignaccount.AccountId,
                Message = notifType.GetMessageText(username, campaignaccount.CampaignId.ToString()),
                Status = NotificationStatus.Created
            });
            return true;
        }


        #endregion
    }
}
