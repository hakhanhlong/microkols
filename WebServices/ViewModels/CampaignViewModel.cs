using Common.Extensions;
using Core.Entities;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.ViewModels
{
    public class BaseCampaignViewModel
    {

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Yêu cầu của chiến dịch")]
        public string Description { get; set; }

    }
   
    public class CampaignViewModel : BaseCampaignViewModel
    {
        public CampaignViewModel()
        {

        }

        public CampaignViewModel(Campaign campaign)
        {
            Id = campaign.Id;
            Description = campaign.Description;
            Type = campaign.Type;
            Status = campaign.Status;
            AccountTypes = campaign.CampaignAccountType.Select(m => m.AccountType).ToList();
            DateStart = campaign.DateStart;
            DateEnd = campaign.DateEnd;
            ExecutionEnd = campaign.ExecutionEnd;
            ExecutionStart = campaign.ExecutionStart;
            DateCreated = campaign.DateCreated;
            UserCreated = campaign.UserCreated;
            CountOption = campaign.CampaignOption.Count();
            Image = campaign.Image.ToListString();
            Data = campaign.Data;
            Code = campaign.Code;
            Title = campaign.Title;
            Quantity = campaign.Quantity;
            Requirement = campaign.Requirement;
            SampleContentText = campaign.SampleContentText;
            ServiceChargePercent = campaign.ServiceChargePercent;


            SampleContent = campaign.SampleContent.ToListString();
            Hashtag = campaign.Hashtag.ToListString();

            var genderOpt = campaign.CampaignOption.FirstOrDefault(m => m.Name == CampaignOptionName.Gender);
            if (genderOpt != null)
            {
                Gender = genderOpt.Value.ToEnum<Gender>();
            }

            var ageRangeOpt = campaign.CampaignOption.FirstOrDefault(m => m.Name == CampaignOptionName.AgeRange);
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

            var cityOpts = campaign.CampaignOption.Where(m => m.Name == CampaignOptionName.City);
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

            var categoryOpts = campaign.CampaignOption.Where(m => m.Name == CampaignOptionName.Category).ToList();
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

            var childOpt = campaign.CampaignOption.FirstOrDefault(m => m.Name == CampaignOptionName.Child);
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


        public static List<CampaignViewModel> GetList(IEnumerable<Campaign> campaigns)
        {
            return campaigns.Select(m => new CampaignViewModel(m)).ToList();
        }


        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Requirement { get; set; }

        public List<string> Image { get; set; } = new List<string>();
        public string Data { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? ExecutionStart { get; set; }

        public DateTime? ExecutionEnd { get; set; }
        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public int CountOption { get; set; }
        public Gender? Gender { get; set; }
        public int? AgeStart { get; set; }
        public int? AgeEnd { get; set; }
        public List<int> CityId { get; set; }
        public List<int> CategoryIds { get; set; }

        public int? ChildType { get; set; }
        public int? ChildAgeMin { get; set; }
        public int? ChildAgeMax { get; set; }

        public string UserCreated { get; set; }
        public List<AccountType> AccountTypes { get;set; }
        public CampaignType Type { get; set; }

        public CampaignStatus Status { get; set; }

        public List<string> Hashtag { get; set; }
        public List<string> SampleContent { get; set; }
        public string SampleContentText { get; set; }


        //longhk addition
        public int ServiceChargePercent { get; set; }
    }

   

    public class ListCampaignViewModel
    {
        public List<CampaignViewModel> Campaigns { get; set; }
        public PagerViewModel Pager { get; set; }
    }




}
