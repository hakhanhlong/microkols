
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
        private readonly IAsyncRepository<CampaignAccountCaption> _CampaignAccountCaptionRepository;
        public CampaignAccountCaptionService(IAsyncRepository<CampaignAccountCaption> CampaignAccountCaptionRepository)
        {
            _CampaignAccountCaptionRepository = CampaignAccountCaptionRepository;
        }

        #region CampaignAccountCaption
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
        public async Task<ListCampaignAccountCaptionViewModel> GetCampaignAccountCaptionsByCampaignId(int campaignId, string order, int page, int pagesize)
        {


            var filter = new CampaignAccountCaptionByCampaignIdSpecification(campaignId);

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


        public async Task<int> CreateCampaignAccountCaption(CreateCampaignAccountCaptionViewModel model, string username)
        {
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

            CampaignAccountCaption.Status = status;
            CampaignAccountCaption.UserModified = username;
            CampaignAccountCaption.DateModified = DateTime.Now;
            await _CampaignAccountCaptionRepository.UpdateAsync(CampaignAccountCaption);

            return true;
        }


        #endregion
    }
}
