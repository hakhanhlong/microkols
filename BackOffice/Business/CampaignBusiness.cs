using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business
{
    public class CampaignBusiness: ICampaignBusiness
    {
        private readonly ILogger<CampaignBusiness> _logger;
        private readonly ICampaignRepository _ICampaignRepository;

        public CampaignBusiness(ILoggerFactory _loggerFactory, ICampaignRepository __ICampaignRepository)
        {
            _logger = _loggerFactory.CreateLogger<CampaignBusiness>();
            _ICampaignRepository = __ICampaignRepository;
        }


        public ListCampaignViewModel GetListCampaign(int pageindex, int pagesize)
        {
            var agencies = _ICampaignRepository.ListPaging("DateModified_desc", pageindex, pagesize);
            var total = _ICampaignRepository.CountAll();


            return new ListCampaignViewModel()
            {
                Campaigns = agencies.Select(a => new CampaignViewModel(a)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }

    }
}
