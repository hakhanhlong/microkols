using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;
using Common.Extensions;

namespace BackOffice.Models
{


    public class ListCampaignViewModel
    {
        public List<CampaignViewModel> Campaigns { get; set; }
        public PagerViewModel Pager { get; set; }
    }

    public class CampaignDetailsViewModel : CampaignViewModel
    {
        public CampaignDetailsViewModel(Campaign campaign,
            IEnumerable<CampaignOption> campaignOptions,
            IEnumerable<CampaignAccount> campaignAccounts,
            IEnumerable<Transaction> transactions) : base(campaign)
        {
            EnabledAccountChargeExtra = campaign.EnabledAccountChargeExtra;

            AccountChargeTime = campaign.AccountChargeTime;

            campaignAccounts = campaignAccounts.Where(m => m.Status != CampaignAccountStatus.Canceled);
            Payment = new CampaignPaymentModel(campaign, campaignOptions, campaignAccounts, transactions);
            Transactions = TransactionViewModel.GetList(transactions);
            CampaignAccounts = CampaignAccountViewModel.GetList(campaignAccounts);
        }

        public bool EnabledAccountChargeExtra { get; set; }
        public int AccountChargeTime { get; set; }


        public CampaignPaymentModel Payment { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }

        public List<CampaignAccountViewModel> CampaignAccounts { get; set; }

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
            CountOption = c.CampaignOption.Count();
            AccountTypes = c.CampaignAccountType.Select(m => m.AccountType).ToList();
            Agency = c.Agency;
            Title = c.Title;
            Description = c.Description;
            Data = c.Data;
            Image = c.Image.ToListString();
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

            TypeToText = c.Type.ToText();

            var genderOpt = c.CampaignOption.FirstOrDefault(m => m.Name == CampaignOptionName.Gender);
            if (genderOpt != null)
            {
                Gender = genderOpt.Value.ToEnum<Gender>();
            }

            var ageRangeOpt = c.CampaignOption.FirstOrDefault(m => m.Name == CampaignOptionName.AgeRange);
            if (ageRangeOpt != null)
            {
                var arrAge = ageRangeOpt.Value.Split('-');
                if (arrAge.Length == 2)
                {
                    var ageStart = 0;
                    var ageEnd = 0;
                    if (int.TryParse(arrAge[0], out ageStart) && int.TryParse(arrAge[1], out ageEnd))
                    {
                        AgeStart = ageStart;
                        AgeEnd = ageEnd;
                    }
                }
            }

            var cityOpts = c.CampaignOption.Where(m => m.Name == CampaignOptionName.City);
            var cityids = new List<int>();
            foreach (var cityOpt in cityOpts)
            {

                var cityid = 0;
                if (int.TryParse(cityOpt.Value, out cityid))
                {
                    cityids.Add(cityid);
                }
            }
            CityId = cityids;

            var categoryOpts = c.CampaignOption.Where(m => m.Name == CampaignOptionName.Category).ToList();
            var categoryids = new List<int>();
            if (categoryOpts.Count > 0)
            {
                foreach (var categoryOpt in categoryOpts)
                {
                    var categoryid = 0;
                    if (int.TryParse(categoryOpt.Value, out categoryid))
                    {
                        categoryids.Add(categoryid);
                    }
                }
            }

            CategoryIds = categoryids;

            var childOpt = c.CampaignOption.FirstOrDefault(m => m.Name == CampaignOptionName.Child);
            if (childOpt != null)
            {



                var arrAge = childOpt.Value.Split('|');
                if (arrAge.Length == 2)
                {
                    var childType = 0;
                    if (int.TryParse(arrAge[0], out childType))
                    {
                        ChildType = childType;
                    }

                    var arrAge2 = arrAge[1].Split('-');
                    if (arrAge2.Length == 2)
                    {
                        var childAgeMin = 0;
                        var childAgeMax = 0;
                        if (int.TryParse(arrAge2[0], out childAgeMin) && int.TryParse(arrAge2[1], out childAgeMax))
                        {
                            ChildAgeMin = childAgeMin;
                            ChildAgeMax = childAgeMax;
                        }
                    }
                }
            }


        }

        public List<AccountType> AccountTypes { get; set; }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        public bool Published { get; set; }

        public bool Deleted { get; set; }

        public Gender? Gender { get; set; }

        public string Code { get; set; }
        public int AgencyId { get; set; }
        public Agency Agency { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int CountOption { get; set; }

        public int? AgeStart { get; set; }
        public int? AgeEnd { get; set; }

        public string Data { get; set; }
        public List<string> Image { get; set; } = new List<string>();

        public string Requirement { get; set; }

        public string SystemNote { get; set; }
        
        public int ServiceChargePercent { get; set; }
        
        public int ExtraOptionChargePercent { get; set; }

        public CampaignType Type { get; set; }

        public string TypeToText { get; set; }

        public int ServiceChargeAmount { get; set; }
        public int AccountChargeAmount { get; set; }
        public int AccountChargeExtraPercent { get; set; }
        public bool EnabledAccountChargeExtra { get; set; }
        public int AccountChargeTime { get; set; }

        public CampaignStatus Status { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public List<int> CityId { get; set; }
        public List<int> CategoryIds { get; set; }

        public int? ChildType { get; set; }
        public int? ChildAgeMin { get; set; }
        public int? ChildAgeMax { get; set; }


        public string CustomKolNames { get; set; }
        public DateTime? AccountFeedbackBefore { get; set; }

        public int Quantity { get; set; }       
    }
}
