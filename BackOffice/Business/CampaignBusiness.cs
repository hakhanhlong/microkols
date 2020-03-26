using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICampaignAccountRepository _ICampaignAccountRepository;
        private readonly IAccountBusiness _IAccountBusiness;
        private readonly IAccountRepository _IAccountRepository;


        public CampaignBusiness(ILoggerFactory _loggerFactory, ICampaignRepository __ICampaignRepository, 
            ITransactionRepository __ITransactionRepository, ICampaignAccountRepository __ICampaignAccountRepository, IAccountBusiness __IAccountBusiness, IAccountRepository __IAccountRepository)
        {
            _logger = _loggerFactory.CreateLogger<CampaignBusiness>();
            _ICampaignRepository = __ICampaignRepository;
            _ITransactionRepository = __ITransactionRepository;
            _ICampaignAccountRepository = __ICampaignAccountRepository;
            _IAccountBusiness = __IAccountBusiness;
            _IAccountRepository = __IAccountRepository;
        }


        public async Task<ListCampaignViewModel> GetListCampaignByAgency(int agencyid, int pageindex, int pagesize)
        {
            var filter = new CampaignByAgencySpecification(agencyid);
            var campaigns = await _ICampaignRepository.ListPagedAsync(filter, "DateModified_desc", pageindex, pagesize);
            var total = await _ICampaignRepository.CountAsync(filter);

            return new ListCampaignViewModel()
            {
                Campaigns = campaigns.Select(a => new CampaignViewModel(a)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }

        public ListCampaignViewModel GetListCampaign(int pageindex, int pagesize)
        {
            var campaigns = _ICampaignRepository.ListPaging("DateModified_desc", pageindex, pagesize);
            var total = _ICampaignRepository.CountAll();


            return new ListCampaignViewModel()
            {
                Campaigns = campaigns.Select(a => new CampaignViewModel(a)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }

        public ListCampaignViewModel Search(string kw, CampaignType? type, CampaignStatus? status, int pageindex, int pagesize)
        {
            var filter = new CampaignSearchSpecification(kw, type, status);

            var agencies = _ICampaignRepository.ListPaged(filter, "DateModified_desc", pageindex, pagesize);
            var total = _ICampaignRepository.Count(filter);


            return new ListCampaignViewModel()
            {
                Campaigns = agencies.Select(a => new CampaignViewModel(a)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }


        public ListCampaignViewModel Search(string kw, CampaignType? type, CampaignStatus? status, DateTime? StartDate, DateTime? EndDate, int pageindex, int pagesize)
        {
            var filter = new CampaignSearchSpecification(kw, type, status, StartDate, EndDate);

            var agencies = _ICampaignRepository.ListPaged(filter, "DateModified_desc", pageindex, pagesize);

            var total = _ICampaignRepository.Count(filter);


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

        public async Task<CampaignDetailsViewModel> GetCampaign(int campaignid)
        {
            var filter = new CampaignSpecification(campaignid);
            var campaign = await _ICampaignRepository.GetSingleBySpecAsync(filter);
            if (campaign != null)
            {
                var transactions = await _ITransactionRepository.ListAsync(new TransactionByCampaignSpecification(campaign.Id));
                return new CampaignDetailsViewModel(campaign, campaign.CampaignOption,
                    campaign.CampaignAccount, transactions);
            }
            return null;
        }


        //public async Task<ListCampaignWithAccountViewModel> GetListCampaignByAllAccount(int type, string keyword, int page, int pagesize)
        //{
        //    var query = await _ICampaignRepository.QueryCampaignByAllAccount(type, keyword);

        //    var total = await query.CountAsync();
        //    var campaigns = await query.OrderByDescending(m => m.Id).GetPagedAsync(page, pagesize);

        //    //var filter = new CampaignByAccountSpecification(accountid, keyword);
        //    //var campaigns = await _campaignRepository.ListPagedAsync(filter, "", page, pagesize);
        //    //var total = await _campaignRepository.CountAsync(filter);


        //    var list = new List<CampaignWithAccountViewModel>();

        //    foreach (var campaign in campaigns)
        //    {

        //        var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(new CampaignAccountByAccountSpecification(accountid, campaign.Id));
        //        if (campaignAccount != null)
        //        {
        //            list.Add(new CampaignWithAccountViewModel(campaign, campaignAccount));
        //        }
        //    }

        //    return new ListCampaignWithAccountViewModel()
        //    {
        //        Campaigns = list,
        //        Pager = new PagerViewModel(page, pagesize, total)
        //    };
        //}


        public ListCampaignWithAccountViewModel GetCampaignAccountByStatus(CampaignAccountStatus status, int pageindex, int pagesize)
        {
            var filter = new CampaignAccountByStatusSpecification(status);
            var campaignAccounts = _ICampaignAccountRepository.ListPaged(filter, "DateModified_desc", pageindex, pagesize);
            var total = _ICampaignAccountRepository.Count(filter);
            var list = new List<CampaignWithAccountViewModel>();
            foreach (var campaignAccount in campaignAccounts)
            {
                list.Add(new CampaignWithAccountViewModel(campaignAccount.Campaign, campaignAccount));
            }

            return new ListCampaignWithAccountViewModel()
            {
                Campaigns = list,
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };

        }

        public ListCampaignWithAccountViewModel GetCampaignAccountByStatus(CampaignAccountStatus? status, DateTime? StartDate, DateTime? EndDate, int pageindex, int pagesize)
        {
            var filter = new CampaignAccountByStatusSpecification(status, StartDate, EndDate);
            var campaignAccounts = _ICampaignAccountRepository.ListPaged(filter, "DateModified_desc", pageindex, pagesize);
            var total = _ICampaignAccountRepository.Count(filter);
            var list = new List<CampaignWithAccountViewModel>();
            foreach (var campaignAccount in campaignAccounts)
            {
                list.Add(new CampaignWithAccountViewModel(campaignAccount.Campaign, campaignAccount));
            }

            return new ListCampaignWithAccountViewModel()
            {
                Campaigns = list,
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };

        }




        public ListCampaignWithAccountViewModel GetCampaignAccountByAccount(CampaignAccountStatus? status, int accountid, int pageindex, int pagesize)
        {
            var filter = new CampaignAccountSpecification(status, accountid);

            var campaignAccounts = _ICampaignAccountRepository.ListPaged(filter, "DateModified_desc", pageindex, pagesize);
            var total = _ICampaignAccountRepository.Count(filter);
            var list = new List<CampaignWithAccountViewModel>();
            foreach (var campaignAccount in campaignAccounts)
            {
                list.Add(new CampaignWithAccountViewModel(campaignAccount.Campaign, campaignAccount));
            }

            return new ListCampaignWithAccountViewModel()
            {
                Campaigns = list,
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };

        }


        public ListCampaignWithAccountViewModel GetCampaignAccountByAccount(CampaignAccountStatus? status, int accountid, DateTime? StartDate, DateTime? EndDate, int pageindex, int pagesize)
        {
            var filter = new CampaignAccountSpecification(status, StartDate, EndDate, accountid);

            var campaignAccounts = _ICampaignAccountRepository.ListPaged(filter, "DateModified_desc", pageindex, pagesize);
            var total = _ICampaignAccountRepository.Count(filter);
            var list = new List<CampaignWithAccountViewModel>();
            foreach (var campaignAccount in campaignAccounts)
            {
                list.Add(new CampaignWithAccountViewModel(campaignAccount.Campaign, campaignAccount));
            }

            return new ListCampaignWithAccountViewModel()
            {
                Campaigns = list,
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };

        }



    }
}
