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
    public class CreateCampaignViewModel
    {
        public Campaign GetEntity(int agencyid, CampaignTypeCharge campaignTypeCharge, Core.Models.SettingModel setting, string code, string username)
        {
            var accountChargeAmount = 0;
            if (Type == CampaignType.CustomService || Type == CampaignType.JoinEvent)
            {
                accountChargeAmount = AccountChargeAmount ?? 0;

            }
            else
            {
                accountChargeAmount = campaignTypeCharge.AccountChargeAmount;
            }


            var accountChargeExtraPercent = 0;

            if (Type == CampaignType.ShareContent || Type == CampaignType.ShareContentWithCaption)
            {
                if (EnabledExtraType)
                {
                    accountChargeExtraPercent = campaignTypeCharge.AccountChargeExtraPercent;
                }
            }

            return new Campaign()
            {
                DateCreated = DateTime.Now,
                AgencyId = agencyid,
                Data = Data,
                DateModified = DateTime.Now,
                Deleted = false,
                Description = Description,
                Image = Type== CampaignType.ChangeAvatar ? Image : string.Empty,
                Published = true,
                Status = CampaignStatus.WaitToConfirm,
                Title = Title,
                UserCreated = username,
                UserModified = username,
                ExtraOptionChargePercent = setting.CampaignExtraOptionChargePercent,
                ServiceChargePercent = setting.CampaignServiceChargePercent,
                ServiceChargeAmount = campaignTypeCharge.ServiceChargeAmount,
                AccountChargeExtraPercent = accountChargeExtraPercent,
                AccountChargeAmount = accountChargeAmount,
                EnabledAccountChargeExtra = Type == CampaignType.ShareContent || Type == CampaignType.ShareContentWithCaption ? EnabledExtraType : false,
                Requirement = Type == CampaignType.CustomService ? Requirement : string.Empty,
                Type = Type,
                AccountChargeTime = AccountChargeTime ?? 1,
                Code = code,
                Quantity = Quantity,
                
                


            };

        }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên chiến dịch")]
        public string Title { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

     
        [Display(Name = "Thông tin đối tượng chiến dịch (Link, hình ảnh,...)")]
        public string Data { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Đối tượng")]
        public List<AccountType> AccountType { get; set; } = new List<AccountType>();



        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }


        [Display(Name = "Chi phí")]
        public int? AccountChargeAmount { get; set; }

        [Display(Name = "Thời gian")]
        public int? AccountChargeTime { get; set; } = 1;

        public string Image { get; set; } = string.Empty;

        [Display(Prompt = "Nhập yêu cầu cụ thể chiến dịch")]
        public string Requirement { get; set; }


        [Display(Name = "Loại chiến dịch")]
        public CampaignType Type { get; set; }

        [Display(Name = "Đính kèm hình ảnh cá nhân")]
        public bool EnabledExtraType { get; set; }



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

        

        [Display(Name = "Chọn khu vực")]
        public int? CityId { get; set; }

        [Display(Name = "Mã chiến dịch")]
        public string Code { get; set; }
    }

}
