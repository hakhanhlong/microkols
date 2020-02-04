using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;
namespace WebServices.ViewModels
{
    public class CampaignWithAccountViewModel : CampaignViewModel
    {
        public CampaignWithAccountViewModel()
        {

        }
        public CampaignWithAccountViewModel(Campaign campaign, CampaignAccount campaignAccount) : base(campaign)
        {
            if (campaignAccount != null)
            {
                CampaignAccount = new CampaignAccountByAccountViewModel(campaignAccount, campaign);
            }

        }

        public CampaignAccountByAccountViewModel CampaignAccount { get; set; } = null;

    }
    public class CampaignAccountByAccountViewModel : CampaignAccountViewModel
    {
        public CampaignAccountByAccountViewModel(CampaignAccount campaignAccount, Campaign campaign) : base(campaignAccount)
        {

            AccountChargeAmount = campaign.GetAccountChagreAmount(campaignAccount);

        }
        public static List<CampaignAccountByAccountViewModel> GetList(IEnumerable<CampaignAccount> campaignAccounts, IEnumerable<Campaign> campaigns)
        {
            var result = new List<CampaignAccountByAccountViewModel>();

            foreach (var item in campaignAccounts)
            {
                var itemCampaign = campaigns.FirstOrDefault(m => m.Id == item.CampaignId);
                if (itemCampaign != null)
                {
                    result.Add(new CampaignAccountByAccountViewModel(item, itemCampaign));
                }
            }
            return result;
        }


    }

    public class ListCampaignWithAccountViewModel
    {
        public List<CampaignWithAccountViewModel> Campaigns { get; set; }
        public PagerViewModel Pager { get; set; }
    }
    public class ListMarketPlaceViewModel
    {
        public List<MarketPlaceViewModel> MarketPlaces { get; set; }
        public PagerViewModel Pager { get; set; }

    }
    public class MarketPlaceViewModel
    {
        public MarketPlaceViewModel(Campaign campaign, List<CampaignAccount> campaignAccounts, Agency agency)
        {
            Campaign = campaign;
            CampaignAccounts = campaignAccounts;
            Agency = agency;
        }
        public static List<MarketPlaceViewModel> GetList(List<Campaign> campaign, List<CampaignAccount> campaignAccounts, List<Agency> agency)
        {
            var result = new List<MarketPlaceViewModel>();


            foreach(var item in campaign)
            {
                var itemAccount = campaignAccounts.Where(m => m.CampaignId == item.Id).ToList();
                var itemAgency = agency.FirstOrDefault(m => m.Id == item.AgencyId);

                result.Add(new MarketPlaceViewModel(item, itemAccount, itemAgency));

            }

            return result;

        }
        public Campaign Campaign { get; set; }
        public List<CampaignAccount> CampaignAccounts { get; set; }
        public Agency Agency { get; set; }

        public int Amount { get; set; } = 100000;
        public int CountApplied
        {
            get
            {
                return CampaignAccounts.Count;
            }
        }
        public int CountAccepted
        {
            get
            {
                var arr = new List<CampaignAccountStatus>() { CampaignAccountStatus.Canceled, CampaignAccountStatus.AgencyRequest, CampaignAccountStatus.AccountRequest };

                return CampaignAccounts.Where(m => !arr.Contains(m.Status)).Count();
            }
        }
    }
}
