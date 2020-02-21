
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
        public CampaignAccountContentService(IAsyncRepository<CampaignAccountContent> CampaignAccountContentRepository)
        {
            _CampaignAccountContentRepository = CampaignAccountContentRepository;
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

            CampaignAccountContent.Status = status;
            CampaignAccountContent.UserModified = username;
            CampaignAccountContent.DateModified = DateTime.Now;
            await _CampaignAccountContentRepository.UpdateAsync(CampaignAccountContent);

            return true;
        }


        #endregion
    }
}
