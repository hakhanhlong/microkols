using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{


    public class ListCampaignTypeChargeViewModel
    {
        public List<CampaignTypeChargeViewModel> ListCampaignTypeCharge { get; set; }
        public PagerViewModel Pager { get; set; }
    }

    public class CampaignTypeChargeViewModel
    {

        public CampaignTypeChargeViewModel(CampaignTypeCharge _CampaignTypeCharge)
        {
            Id = _CampaignTypeCharge.Id;
            Type = _CampaignTypeCharge.Type;
            ServiceChargeAmount = _CampaignTypeCharge.ServiceChargeAmount;
            AccountChargeAmount = _CampaignTypeCharge.AccountChargeAmount;
            AccountChargeExtraPercent = _CampaignTypeCharge.AccountChargeExtraPercent;

        }

        public CampaignTypeChargeViewModel() { }

        public int Id { get; set; }
        public CampaignType Type { get; set; }
        public int ServiceChargeAmount { get; set; }
        public int AccountChargeAmount { get; set; }
        public int AccountChargeExtraPercent { get; set; }




    }

}
