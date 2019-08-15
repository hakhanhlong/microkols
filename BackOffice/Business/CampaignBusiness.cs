using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Interfaces;
using Core.Specifications;
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
        private readonly ITransactionRepository _ITransactionRepository;

        public CampaignBusiness(ILoggerFactory _loggerFactory, ICampaignRepository __ICampaignRepository, ITransactionRepository __ITransactionRepository)
        {
            _logger = _loggerFactory.CreateLogger<CampaignBusiness>();
            _ICampaignRepository = __ICampaignRepository;
            _ITransactionRepository = __ITransactionRepository;
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

        public async Task<CampaignDetailsViewModel> GetCampaign(int agencyid, int campaignid)
        {
            var filter = new CampaignByAgencySpecification(agencyid, campaignid);
            var campaign = await _ICampaignRepository.GetSingleBySpecAsync(filter);
            if (campaign != null)
            {
                var transactions = await _ITransactionRepository.ListAsync(new TransactionByCampaignSpecification(campaign.Id));
                return new CampaignDetailsViewModel(campaign, campaign.CampaignOption,
                    campaign.CampaignAccount, transactions);
            }
            return null;
        }

    }
}
