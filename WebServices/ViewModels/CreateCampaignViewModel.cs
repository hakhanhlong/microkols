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
    public class CreateCampaignViewModel
    {
        public Campaign GetEntity(int agencyid, CampaignTypeCharge campaignTypeCharge, Core.Models.SettingModel setting, string code, string username)
        {
            //var accountChargeAmount = 0;
            //if (Type == CampaignType.CustomService || Type == CampaignType.JoinEvent)
            //{
            //    accountChargeAmount = AccountChargeAmount ?? 0;
            //}
            //else
            //{
            //    accountChargeAmount = campaignTypeCharge.AccountChargeAmount;
            //}


            //var accountChargeExtraPercent = 0;

            //if (Type == CampaignType.ShareContent || Type == CampaignType.ShareContentWithCaption)
            //{
            //    if (EnabledExtraType)
            //    {
            //        accountChargeExtraPercent = campaignTypeCharge.AccountChargeExtraPercent;
            //    }
            //}

            var start = "";
            var end = "";
            if (!string.IsNullOrEmpty(ExecutionTime))
            {
                var arrDate = ExecutionTime.Split('-');
                if(arrDate.Length== 2)
                {
                    start = arrDate[0].Trim();
                    end = arrDate[1].Trim();
                }
            }
            var image = string.Empty;

            if(Type == CampaignType.ChangeAvatar)
            {
                image = Image;
            }
            else if(Type== CampaignType.ShareContentWithCaption)
            {
                image = AddonImages.ToListString();
            }
            return new Campaign()
            {
                DateCreated = DateTime.Now,
                AgencyId = agencyid,
                Data = Data,
                DateModified = DateTime.Now,
                Deleted = false,
                Description = Description,
                Image = image,
                Published = true,
                //Status = CampaignStatus.Created, // cap nhat status da duyet luon de facebook check,
                Status = CampaignStatus.Confirmed,
                Title = Title,
                UserCreated = username,
                UserModified = username,
                ExtraOptionChargePercent = setting.CampaignExtraOptionChargePercent,
                ServiceChargePercent = setting.CampaignServiceChargePercent,
                ServiceVATPercent = setting.CampaignVATChargePercent,
                ServiceChargeAmount =0,
                AccountChargeExtraPercent = 0,
                AccountChargeAmount = 0,
                EnabledAccountChargeExtra = false,
                AccountChargeTime = 0,
                Requirement = Type == CampaignType.CustomService ? Requirement : string.Empty,
                Type = Type,
                
                Code = code,
                Quantity = Quantity,
                DateStart = start.ToViDateTime(),
                DateEnd = end.ToViDateTime(),
                AccountFeedbackBefore = FeedbackBefore.ToViDateTime(),
                CustomKolNames = CustomKolNames.ToListString(),
                Method = Method,
                SampleContent = SampleContent.ToListString(),
                Hashtag = HashTag.ToListString(),
                SampleContentText   = SampleContentText

            };

        }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên chiến dịch")]
        public string Title { get; set; }

        [Display(Name = "Nội dung, Mô tả thời gian cụ thể chiến dịch")]
        public string Description { get; set; }

     
        [Display(Name = "Thông tin đối tượng chiến dịch (Link, hình ảnh,...)")]
        public string Data { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Bạn cần Micro Kols")]
        public List<AccountType> AccountType { get; set; } = new List<AccountType>();



        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }


        [Display(Name = "Chi phí")]
        public int? AccountChargeAmount { get; set; }

        [Display(Name = "Thời gian")]
        public int? AccountChargeTime { get; set; } = 1;

        public string Image { get; set; } = string.Empty;



        [Display(Name = "Hình ảnh đính kèm")]
        public List<string> AddonImages { get; set; } = new List<string>();

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


        [Display(Name = "Tags")]
        public bool EnabledTags { get; set; } = false;
        [Display(Name = "Thêm Tags")]
        public List<string> AccountTags { get; set; }



        [Display(Name = "Khu vực")]
        public bool EnabledCity { get; set; } = false;

   
        

        [Display(Name = "Chọn khu vực")]
        public List<int> CityId { get; set; }

        [Display(Name = "Mã chiến dịch")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Thời gian thực hiện")]
        public string ExecutionTime { get; set; }


        [Display(Name = "Phản hồi trước")]
        public string FeedbackBefore { get; set; }


        [Display(Name = "Kols mà bạn muốn hợp tác")]
        public List<string> CustomKolNames { get; set; }

        public List<int> AccountIds { get; set; }
        public List<int> AccountChargeAmounts { get; set; }


        [Display(Name = "Giới tính")]
        public int? ChildType { get; set; }

        [Display(Name = "Độ tuổi")]
        public int? ChildAgeMin { get; set; }
        public int? ChildAgeMax { get; set; }


        public List<string> HashTag { get; set; }


        [Display(Name = "Hình ảnh chiến dịch cung cấp")]
        public List<string> SampleContent { get; set; }

        [Display(Name = "Nội dung mẫu")]
        public string SampleContentText { get; set; }
        [Display(Name = "Phương thức")]
        public CampaignMethod Method { get; set; } = CampaignMethod.OpenJoined;
    }

}
