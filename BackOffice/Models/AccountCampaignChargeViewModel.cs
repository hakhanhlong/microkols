using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{
    public class AccountCampaignChargeViewModel
    {
        public AccountCampaignChargeViewModel() { }

        public AccountCampaignChargeViewModel(AccountCampaignCharge model) {

            Id = model.Id;
            AccountId = model.AccountId;
            Type = model.Type;
            AccountChargeAmount = model.AccountChargeAmount;

        }

        public int Id { get; set; }
        public int AccountId { get; set; }

        public CampaignType Type { get; set; }
        public int AccountChargeAmount { get; set; }

    }
}
