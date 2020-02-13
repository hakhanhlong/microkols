using Common.Extensions;
using Common.Helpers;
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
                if (arrDate.Length == 2)
                {
                    start = arrDate[0].Trim();
                    end = arrDate[1].Trim();
                }
            }
            var image = string.Empty;

            if (Type == CampaignType.ChangeAvatar)
            {
                image = Image;
            }
            else if (Type == CampaignType.ShareContentWithCaption)
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
                ServiceChargeAmount = 0,
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
                SampleContentText = SampleContentText

            };

        }

        public static Campaign GetEntity(int agencyid, CreateCampaignInfoViewModel info, CreateCampaignTargetViewModel target, CampaignTypeCharge campaignTypeCharge, Core.Models.SettingModel setting, string code, string username)
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
         
            var executionTime = DateRangeHelper.GetDateRange(target.ExecutionTime);
            var regTime = DateRangeHelper.GetDateRange(target.RegisterTime);
            var feedbackTime = DateRangeHelper.GetDateRange(target.FeedbackBefore);

            var image = string.Empty;

            if (info.Type == CampaignType.ChangeAvatar)
            {
                image = info.Image;
            }
            else if (info.Type == CampaignType.ShareContentWithCaption)
            {
                image = info.AddonImages.ToListString();
            }
            return new Campaign()
            {
                DateCreated = DateTime.Now,
                AgencyId = agencyid,
                Data = info.Data,
                DateModified = DateTime.Now,
                Deleted = false,
                Description = info.Description,
                Image = image,
                Published = true,
                Status = CampaignStatus.Created, // cap nhat status da duyet luon de facebook check,
                //Status = CampaignStatus.Confirmed,
                Title = info.Title,
                UserCreated = username,
                UserModified = username,
                ExtraOptionChargePercent = setting.CampaignExtraOptionChargePercent,
                ServiceChargePercent = setting.CampaignServiceChargePercent,
                ServiceVATPercent = setting.CampaignVATChargePercent,
                ServiceChargeAmount = 0,
                AccountChargeExtraPercent = 0,
                AccountChargeAmount = 0,
                EnabledAccountChargeExtra = false,
                AccountChargeTime = 0,
                Requirement = info.Type == CampaignType.CustomService ? info.Requirement : string.Empty,
                Type = info.Type,
                Code = code,
                Quantity =target.Quantity,
                DateStart = regTime != null ? (DateTime?) regTime.Value.Start:null,
                DateEnd = regTime != null ? (DateTime?)regTime.Value.End : null,
                //AccountFeedbackBefore = target.FeedbackBefore.ToViDateTime(),
                CustomKolNames = target.CustomKolNames.ToListString(),
                Method = CampaignMethod.OpenJoined,
                SampleContent = info.SampleContent.ToListString(),
                Hashtag =info.HashTag.ToListString(),
                SampleContentText = info.SampleContentText,
                KPIMin = target.KPIMin,
                InteractiveMin = target.InteractiveMin,
                ExecutionStart = executionTime != null ? (DateTime?)executionTime.Value.Start : null,
                ExecutionEnd = executionTime != null ? (DateTime?)executionTime.Value.End : null,
                FeedbackStart = feedbackTime != null ? (DateTime?)feedbackTime.Value.Start : null,
                FeedbackEnd = feedbackTime != null ? (DateTime?)feedbackTime.Value.End : null,
                AmountMax = target.AmountMax,
                AmountMin = target.AmountMin,
                IsSendProduct = info.SendProduct
               



            };

        }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên chiến dịch")]
        public string Title { get; set; }

        [Display(Name = "Giới thiệu ngắn gọn sản phẩm, dịch vụ chạy chiến dịch")]
        public string Description { get; set; }


        [Display(Name = "Liên kết URL nội dung")]
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


        [Display(Name = "Hình ảnh Avatar")]
        public string Image { get; set; } = string.Empty;



        [Display(Name = "Hình ảnh đính kèm")]
        public List<string> AddonImages { get; set; } = new List<string>();

        [Display(Name = "Yêu cầu của chiến dịch")]
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



    public class CreateCampaignInfoViewModel
    {


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên chiến dịch")]
        public string Title { get; set; }

        [Display(Name = "Giới thiệu ngắn gọn sản phẩm, dịch vụ chạy chiến dịch")]
        public string Description { get; set; }

        [Display(Name = "Liên kết URL nội dung")]
        public string Data { get; set; }

        [Display(Name = "Liên kết URL nội dung")]
        public string Image { get; set; } = string.Empty;


        [Display(Name = "Hình ảnh đính kèm")]
        public List<string> AddonImages { get; set; } = new List<string>();

        [Display(Name = "Yêu cầu cụ thể chiến dịch")]
        public string Requirement { get; set; }

        [Display(Name = "Thời gian")]
        public int? AccountChargeTime { get; set; } = 1;


        [Display(Name = "Loại chiến dịch")]
        public CampaignType Type { get; set; }

        [Display(Name = "Đính kèm hình ảnh cá nhân")]
        public bool EnabledExtraType { get; set; }


        [Display(Name = "Bạn sẽ gửi sản phẩm trải nghiệm cho influencer hoặc mời họ đến trải nghiệm dịch vụ?")]
        public bool SendProduct { get; set; }

        [Display(Name = "Mã chiến dịch")]
        public string Code { get; set; }

        public List<string> HashTag { get; set; }


        [Display(Name = "Hình ảnh chiến dịch cung cấp")]
        public List<string> SampleContent { get; set; }

        [Display(Name = "Nội dung mẫu")]
        public string SampleContentText { get; set; }
        [Display(Name = "Phương thức")]
        public CampaignMethod Method { get; set; } = CampaignMethod.OpenJoined;
    }

    public class CreateCampaignTargetViewModel
    {

        public string InfoModel { get; set; }
        public CampaignType Type { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Bạn cần Micro Kols")]
        public List<AccountType> AccountType { get; set; } = new List<AccountType>();

        public int? AccountChargeAmount { get; set; }


        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Chi phí tối thiểu")]
        public int AmountMin { get; set; }
        [Display(Name = "Chi phí tối đa")]
        public int AmountMax { get; set; }

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

        [Display(Name = "Lựa chọn những Influencer/KOLs mà bạn muốn hợp tác/chỉ định")]
        public bool EnabledAccount { get; set; } = false;
  



        [Display(Name = "KPIs(Like + Share + Comments) tối thiểu mà influencer sẽ phải đạt cho mỗi post để được ghi nhận doanh thu")]
        public int KPIMin { get; set; }

        [Display(Name = "Bạn cần người dùng cam kết lượt tương tác đạt được tối thiểu cho mỗi post sẽ tăng thêm")]
        public int InteractiveMin { get; set; }


        [Display(Name = "Chọn khu vực")]
        public List<int> CityId { get; set; }


        [Display(Name = "Mã chiến dịch")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Thời gian thực hiện")]
        public string ExecutionTime { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Thời gian nhận đăng ký")]
        public string RegisterTime { get; set; }

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

    }
}
