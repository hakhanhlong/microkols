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
        [Display(Name = "Tiêu đề chiến dịch")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Thông tin sản phẩm hoặc dịch vụ")]
        public string Description { get; set; }
    }
    public class CreateCampaignViewModel : BaseCampaignViewModel
    {
        public Campaign GetEntity(int agencyid, CampaignTypePrice campaignTypePrice, Core.Models.SettingModel setting, string username)
        {
            var start = DateStart.ToViDate();
            var end = DateEnd.ToViDate();
            if (start.HasValue && end.HasValue)
            {
                return new Campaign()
                {
                    DateCreated = DateTime.Now,
                    AgencyId = agencyid,
                    DateEnd = end,
                    DateStart = start,
                    Data = string.Empty,
                    DateModified = DateTime.Now,
                    Deleted = false,
                    Description = Description,
                    ServiceChargePercent = setting.ServiceCharge,
                    Image = string.Empty,
                    Published = true,
                    Status = CampaignStatus.Created,
                    Title = Title,
                    UserCreated = username,
                    UserModified = username,
                    ExtraOptionChargePercent = setting.ExtraOptionCharge,
                    AccountExtraPercent = campaignTypePrice.AccountExtraPricePercent,
                    ServicePrice = campaignTypePrice.ServicePrice,
                    AccountPrice = campaignTypePrice.AccountPrice,
                    EnabledAccountExtra = Type == CampaignType.ShareContent || Type == CampaignType.ShareContentWithCaption ? EnabledAccountExtra : false,
                    Requirement = Type == CampaignType.CustomService ? Requirement: string.Empty,
                    Type  = Type



                };

            }
            return null;
        }

        [Display(Name = "Số lượng người bạn cần")]
        public int NumberOfAccount { get; set; }

        [Display(Name = "Chi phí cho mỗi người")]
        public int? AccountPrice { get; set; }

        [Display(Name = "Yêu cầu cụ thể chiến dịch")]
        public string Requirement { get; set; }


        [Display(Name = "Loại chiến dịch")]
        public CampaignType Type { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Bắt đầu")]
        public string DateStart { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Kết thúc")]
        public string DateEnd { get; set; }

        [Display(Name = "Giới tính")]
        public bool EnabledGender { get; set; } = false;

        [Display(Name = "Chọn giới tính")]
        public Gender? Gender { get; set; }

        [Display(Name = "Độ tuổi")]
        public bool EnabledAgeRange { get; set; } = false;

        [Display(Name = "Từ", Prompt = "Từ")]
        public int? AgeStart { get; set; }
        [Display(Name = "Đến", Prompt = "Đến")]
        public int? AgeEnd { get; set; }

        [Display(Name = "Lĩnh vực quan tâm/thế mạnh")]
        public bool EnabledCategory { get; set; } = false;
        [Display(Name = "Chọn lĩnh vực")]
        public List<int> CategoryId { get; set; }



        [Display(Name = "Khu vực")]
        public bool EnabledCity { get; set; } = false;


        [Display(Name = "Đính kèm ảnh cá nhân")]
        public bool EnabledAccountExtra { get; set; } = false;

        [Display(Name = "Chọn khu vực")]
        public int? CityId { get; set; }

    }

    public class CampaignViewModel : BaseCampaignViewModel
    {
        public CampaignViewModel()
        {

        }

        public CampaignViewModel(Campaign campaign)
        {
            Id = campaign.Id;
            Title = campaign.Title;
            Description = campaign.Description;
            Type = campaign.Type;
            Status = campaign.Status;

            DateStart = campaign.DateStart;
            DateEnd = campaign.DateEnd;
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
                    if (int.TryParse(arrAge[0], out ageStart) && int.TryParse(arrAge[0], out ageEnd))
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

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public Gender? Gender { get; set; }

        public int? AgeStart { get; set; }
        public int? AgeEnd { get; set; }
        public int? CityId { get; set; }
        public List<int> CategoryIds { get; set; }

        public CampaignType Type { get; set; }
        public CampaignStatus Status { get; set; }


    }

    public class ListCampaignViewModel
    {
        public List<CampaignViewModel> Campaigns { get; set; }
        public PagerViewModel Pager { get; set; }
    }




}
