using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;
namespace Website.ViewModels
{
    public class CampaignWithAccountViewModel : CampaignViewModel
    {
        public CampaignWithAccountViewModel()
        {

        }
        public CampaignWithAccountViewModel(Campaign campaign, CampaignAccount campaignAccount) : base(campaign)
        {
            CampaignAccount = new CampaignAccountByAccountViewModel(campaignAccount, campaign);
        }

        public CampaignAccountByAccountViewModel CampaignAccount { get; set; }

    }
    public class CampaignAccountByAccountViewModel: CampaignAccountViewModel
    {
        public CampaignAccountByAccountViewModel(CampaignAccount campaignAccount,Campaign campaign):base(campaignAccount)
        {
          
            AccountChargeAmount = campaign.GetAccountChagreAmount(campaignAccount);
            
        }
        public static List<CampaignAccountByAccountViewModel> GetList(IEnumerable<CampaignAccount> campaignAccounts, IEnumerable<Campaign> campaigns)
        {
            var result = new List<CampaignAccountByAccountViewModel>();

            foreach(var item in campaignAccounts)
            {
                var itemCampaign = campaigns.FirstOrDefault(m => m.Id == item.CampaignId);
                if(itemCampaign!= null)
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

}
