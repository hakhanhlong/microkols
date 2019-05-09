using Common.Extensions;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
    public class CreateCampaignViewModel
    {
        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tiêu đề chiến dịch")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Thông tin sản phẩm hoặc dịch vụ")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Bắt đầu")]
        public string DateStart { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Kết thúc")]
        public string DateEnd { get; set; }


        [Display(Name = "Loại chiến dịch")]
        public int CampaignTypeId { get; set; }


        [Display(Name = "Giới tính")]
        public bool EnabledGender { get; set; } = false;

        [Display(Name = "Chọn giới tính")]
        public Gender? Gender { get; set; }

        [Display(Name = "Độ tuổi")]
        public bool EnabledAgeRange { get; set; } = false;

        [Display(Name = "Từ", Prompt ="Từ")]
        public int? AgeStart { get; set; }
        [Display(Name = "Đến", Prompt = "Đến")]
        public int? AgeEnd { get; set; }

        [Display(Name = "Lĩnh vực quan tâm/thế mạnh")]
        public bool EnabledCategory { get; set; } = false;
        [Display(Name = "Chọn lĩnh vực")]
        public List<int> CategoryId { get; set; }



        [Display(Name = "Khu vực")]
        public bool EnabledCity { get; set; } = false;

        [Display(Name = "Chọn khu vực")]
        public int? CityId { get; set; }


        public Campaign GetEntity(int agencyid, CampaignType campaignType, Core.Models.SettingModel setting, string username)
        {
            var start = DateStart.ToViDate();
            var end = DateEnd.ToViDate();
            if (start.HasValue && end.HasValue)
            {
                return new Campaign()
                {
                    DateCreated = DateTime.Now,
                    AgencyId = agencyid,
                    CampaignTypeId = campaignType.Id,
                    CampaignTypeCharge = campaignType.Price,
                    DateEnd = end,
                    DateStart = start,
                    Data = string.Empty,
                    DateModified = DateTime.Now,
                    Deleted = false,
                    Description = Description,
                    ExtraChargePercent = setting.ExtraOptionCharge,
                    ServiceChargePercent = setting.ServiceCharge,
                    Image = string.Empty,
                    Published = true,
                    Status = CampaignStatus.Created,
                    Title = Title,
                    UserCreated = username,
                    UserModified = username
                };

            }
            return null;
        }
    }
}
