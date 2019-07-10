using Common.Extensions;
using Core.Entities;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
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
            DateCreated = campaign.DateCreated;
            UserCreated = campaign.UserCreated;
            CountOption = campaign.CampaignOption.Count();
            Image = campaign.Image;
            Data = campaign.Data;
            Code = campaign.Code;
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

            var cityOpt = campaign.CampaignOption.FirstOrDefault(m => m.Name == CampaignOptionName.City);
            if (cityOpt != null)
            {
                var cityid = 0;
                if (int.TryParse(cityOpt.Value, out cityid))
                {
                    CityId = cityid;
                }
            }

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
        }


        public static List<CampaignViewModel> GetList(IEnumerable<Campaign> campaigns)
        {
            return campaigns.Select(m => new CampaignViewModel(m)).ToList();
        }


        public int Id { get; set; }

        public string Code { get; set; }

        public string Image { get; set; }
        public string Data { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public int CountOption { get; set; }
        public Gender? Gender { get; set; }
        public int? AgeStart { get; set; }
        public int? AgeEnd { get; set; }
        public int? CityId { get; set; }
        public List<int> CategoryIds { get; set; }

        public string UserCreated { get; set; }
        public List<AccountType> AccountTypes { get;set; }
        public CampaignType Type { get; set; }

        public CampaignStatus Status { get; set; }




    }

   

    public class ListCampaignViewModel
    {
        public List<CampaignViewModel> Campaigns { get; set; }
        public PagerViewModel Pager { get; set; }
    }




}
