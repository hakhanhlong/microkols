using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{


    public class ListCampaignViewModel
    {
        public List<CampaignViewModel> Campaigns { get; set; }
        public PagerViewModel Pager { get; set; }
    }

    public class CampaignViewModel
    {
        public CampaignViewModel() { }


        public CampaignViewModel(Campaign c) {
            Id = c.Id;
            DateCreated = c.DateCreated;
            DateModified = c.DateModified;
            UserCreated = c.UserCreated;
            UserModified = c.UserModified;
            Published = c.Published;
            Deleted = c.Deleted;
            Code = c.Code;
            AgencyId = c.AgencyId;
            Agency = c.Agency;
            Title = c.Title;
            Description = c.Description;
            Data = c.Data;
            Image = c.Image;
            Requirement = c.Requirement;
            SystemNote = c.SystemNote;
            ServiceChargePercent = c.ServiceChargePercent;
            ExtraOptionChargePercent = c.ExtraOptionChargePercent;
            Type = c.Type;
            ServiceChargeAmount = c.ServiceChargeAmount;
            AccountChargeAmount = c.AccountChargeAmount;
            AccountChargeExtraPercent = c.AccountChargeExtraPercent;
            EnabledAccountChargeExtra = c.EnabledAccountChargeExtra;
            AccountChargeTime = c.AccountChargeTime;
            Status = c.Status;
            DateStart = c.DateStart;
            DateEnd = c.DateEnd;
            CustomKolNames = c.CustomKolNames;
            AccountFeedbackBefore = c.AccountFeedbackBefore;
            Quantity = c.Quantity;
        }


        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        public bool Published { get; set; }

        public bool Deleted { get; set; }
        

        public string Code { get; set; }
        public int AgencyId { get; set; }
        public Agency Agency { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        
        public string Data { get; set; }
        public string Image { get; set; }

        public string Requirement { get; set; }

        public string SystemNote { get; set; }
        
        public int ServiceChargePercent { get; set; }
        
        public int ExtraOptionChargePercent { get; set; }

        public CampaignType Type { get; set; }
        public int ServiceChargeAmount { get; set; }
        public int AccountChargeAmount { get; set; }
        public int AccountChargeExtraPercent { get; set; }
        public bool EnabledAccountChargeExtra { get; set; }
        public int AccountChargeTime { get; set; }

        public CampaignStatus Status { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }


        public string CustomKolNames { get; set; }
        public DateTime? AccountFeedbackBefore { get; set; }

        public int Quantity { get; set; }

        



    }
}
