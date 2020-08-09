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

            var reviewTime = DateRangeHelper.GetDateRange(info.ReviewDate);


            var image = string.Empty;

            if (!string.IsNullOrEmpty(info.Image))
            {
                image = info.Image;
            }
            else
            {
                image = info.AddonImages.ToListString();
            }

            

            //if (info.Type == CampaignType.ChangeAvatar)
            //{
                
            //}
            //else if (info.Type == CampaignType.ShareContentWithCaption)
            //{
            //    image = info.AddonImages.ToListString();
            //}

            var reviewaddress = "";
            var reviewpayback = false;

            if (info.Type == CampaignType.ReviewProduct && info.ReviewType.HasValue)
            {
                if (info.ReviewType == CampaignReviewType.GuiSanPham)
                {
                    if (1 == info.ReviewPayback)
                    {
                        reviewaddress = info.ReviewAddress;
                        reviewpayback = true;
                    }
                }
                else
                {
                    reviewaddress = info.ReviewAddress2;
                }
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
                Requirement = info.Requirement,
                Type = info.Type,
                Code = code,
                Quantity = target.Quantity,
                DateStart = regTime != null ? (DateTime?)regTime.Value.Start : null,
                DateEnd = regTime != null ? (DateTime?)regTime.Value.End.AddSeconds(59) : null,
                //AccountFeedbackBefore = target.FeedbackBefore.ToViDateTime(),
                CustomKolNames = target.CustomKolNames.ToListString(),
                Method = CampaignMethod.OpenJoined,
                SampleContent = info.SampleContent.ToListString(),
                Hashtag = info.HashTag.ToListString(),
                SampleContentText = info.SampleContentText,
                KPIMin = target.KPIMin,
                //InteractiveMin = target.InteractiveMin,
                InteractiveMin = target.KPIMin + target.InteractiveMin,

                ExecutionStart = executionTime != null ? (DateTime?)executionTime.Value.Start : null,
                ExecutionEnd = executionTime != null ? (DateTime?)executionTime.Value.End.AddSeconds(59) : null,
                FeedbackStart = feedbackTime != null ? (DateTime?)feedbackTime.Value.Start : null,
                FeedbackEnd = feedbackTime != null ? (DateTime?)feedbackTime.Value.End.AddSeconds(59) : null,
                AmountMax = target.AmountMax,
                AmountMin = target.AmountMin,
                IsSendProduct = info.SendProduct,


                ReviewStart = reviewTime != null ? (DateTime?)reviewTime.Value.Start : null,
                ReviewEnd = reviewTime != null ? (DateTime?)reviewTime.Value.End.AddSeconds(59) : null,
                ReviewAddress = reviewaddress,
                ReviewType = info.ReviewType,
                ReviewPayback = reviewpayback




            };

        }


    }



    public class EditCampaignInfoViewModel : CreateCampaignInfoViewModel
    {
        public EditCampaignInfoViewModel()
        {

        }
        public EditCampaignInfoViewModel(Campaign campaign)
        {

            Id = campaign.Id;
            Title = campaign.Title;
            Description = campaign.Description;
            Data = campaign.Data;

            if (!string.IsNullOrEmpty(campaign.Image))
            {
                Image = campaign.Image;
            }
            else
            {
                Image = string.Empty;
            }

            //if (campaign.Type == CampaignType.ChangeAvatar)
            //{
            //    Image = campaign.Image;
            //}
            //else
            //{
            //    Image = string.Empty;
            //}


            if (campaign.Type == CampaignType.ShareContentWithCaption)
            {
                AddonImages = campaign.Image.ToListString();
            }
            else
            {
                AddonImages = new List<string>();
            }
            Requirement = campaign.Requirement;

            AccountChargeTime = campaign.AccountChargeTime;
            Type = campaign.Type;
            SendProduct = campaign.IsSendProduct ?? false;
            Code = campaign.Code;
            HashTag = campaign.Hashtag.ToListString();
            SampleContent = campaign.SampleContent.ToListString();
            SampleContentText = campaign.SampleContentText;
            Method = campaign.Method ?? CampaignMethod.OpenJoined;
            if (campaign.ReviewStart.HasValue && campaign.ReviewEnd.HasValue)
            {
                ReviewDate = $"{campaign.ReviewStart.Value.ToViDateTime()} - {campaign.ReviewEnd.Value.ToViDateTime()}";

                ReviewAddress = campaign.ReviewAddress;
            }
        }
        public int Id { get; set; }

        public Campaign GetEntity(Campaign campaign)
        {
            campaign.Title = Title;
            campaign.Description = Description;
            campaign.Data = Data;
            var image = string.Empty;

            if (!string.IsNullOrEmpty(Image))
            {
                image = Image;
            }
            else
            {               
                image = AddonImages.ToListString();
            }

            //if (Type == CampaignType.ChangeAvatar)
            //{
            //    image = Image;
            //}
            //else if (Type == CampaignType.ShareContentWithCaption)
            //{
            //    image = AddonImages.ToListString();
            //}

            campaign.Image = image;
            campaign.Requirement = Requirement;

            campaign.AccountChargeTime = AccountChargeTime ?? 0;
            campaign.Type = Type;
            campaign.IsSendProduct = SendProduct;
            campaign.Code = campaign.Code;
            campaign.Hashtag = HashTag.ToListString();

            if(!string.IsNullOrEmpty(SampleContent.ToListString()))
            {
                campaign.SampleContent = SampleContent.ToListString();
            }
            
          
            campaign.SampleContentText = SampleContentText;
            campaign.Method = Method;

            var reviewTime = DateRangeHelper.GetDateRange(ReviewDate);


            campaign.ReviewStart = reviewTime != null ? (DateTime?)reviewTime.Value.Start : null;
            campaign.ReviewEnd = reviewTime != null ? (DateTime?)reviewTime.Value.End : null;
            campaign.ReviewAddress = ReviewAddress;
            return campaign;
        }
    }

    public class EditCampaignTargetViewModel : CreateCampaignTargetViewModel
    {
        public EditCampaignTargetViewModel()
        {

        }
        public EditCampaignTargetViewModel(Campaign campaign)
        {
            Id = campaign.Id;
            Type = campaign.Type;
            AccountType = campaign.CampaignAccountType.Select(m => m.AccountType).ToList();
            AccountChargeAmount = campaign.AccountChargeAmount;
            Quantity = campaign.Quantity;
            AmountMin = campaign.AmountMin;
            AmountMax = campaign.AmountMax;
            var options = campaign.CampaignOption.ToList();

            var optionGender = options.Where(m => m.Name == CampaignOptionName.Gender).FirstOrDefault();
            if (optionGender != null)
            {
                if (!string.IsNullOrEmpty(optionGender.Value))
                {
                    EnabledGender = true;
                    if (optionGender.Value == "Male")
                    {
                        Gender = Core.Entities.Gender.Male;
                    }
                    else
                    {
                        Gender = Core.Entities.Gender.Female;
                    }
                }
            }

            var optionAge = options.Where(m => m.Name == CampaignOptionName.AgeRange).FirstOrDefault();
            if (optionAge != null)
            {
                var arr = optionAge.Value.Split('-');
                if (arr.Length == 2)
                {
                    EnabledAgeRange = true;
                    AgeStart = int.Parse(arr[0]);
                    AgeEnd = int.Parse(arr[1]);
                }
            }

            var optionCate = options.Where(m => m.Name == CampaignOptionName.Category).ToList();
            if (optionCate.Count > 0)
            {
                EnabledCategory = true;
                CategoryId = optionCate.Select(m => int.Parse(m.Value)).ToList();
            }


            var optionCity = options.Where(m => m.Name == CampaignOptionName.City).ToList();
            if (optionCity.Count > 0)
            {
                EnabledCity = true;
                CityId = optionCity.Select(m => int.Parse(m.Value)).ToList();
            }


            var campaignAccounts = campaign.CampaignAccount.ToList();
            if (campaignAccounts.Count > 0)
            {
                EnabledAccount = true;
                AccountIds = campaignAccounts.Select(m => m.AccountId).ToList();
            }

            KPIMin = campaign.KPIMin ?? 0;

            InteractiveMin = campaign.InteractiveMin ?? 0;

            

            Code = campaign.Code;
            ExecutionTime = campaign.ExecutionStart.ToDateRange(campaign.ExecutionEnd);
            RegisterTime = campaign.DateStart.ToDateRange(campaign.DateEnd);
            FeedbackBefore = campaign.FeedbackStart.ToDateRange(campaign.FeedbackEnd);
            CustomKolNames = campaign.CustomKolNames.ToListString();
            AccountChargeAmounts = new List<int>();

            var optionChild = options.Where(m => m.Name == CampaignOptionName.Child).FirstOrDefault();

            if (optionChild != null)
            {
                var arr1 = optionChild.Value.Split('|');
                if (arr1.Length == 2)
                {
                    ChildType = int.Parse(arr1[0]);
                    var arr2 = arr1[1].Split('-');
                    if (arr2.Length == 2)
                    {

                        ChildAgeMin = int.Parse(arr2[0]);
                        ChildAgeMax = int.Parse(arr2[1]);
                    }
                }
            }



        }
        public int Id { get; set; }

        internal Campaign GetEntity(Campaign campaign)
        {
            var executionTime = DateRangeHelper.GetDateRange(ExecutionTime);
            var regTime = DateRangeHelper.GetDateRange(RegisterTime);
            var feedbackTime = DateRangeHelper.GetDateRange(FeedbackBefore);


            campaign.Quantity = Quantity;
            campaign.DateStart = regTime != null ? (DateTime?)regTime.Value.Start : null;
            campaign.DateEnd = regTime != null ? (DateTime?)regTime.Value.End : null;
            campaign.CustomKolNames = CustomKolNames.ToListString();
            campaign.KPIMin = KPIMin;

            campaign.InteractiveMin = InteractiveMin;

            //campaign.InteractiveMin = KPIMin + InteractiveMin;

            campaign.ExecutionStart = executionTime != null ? (DateTime?)executionTime.Value.Start : null;
            campaign.ExecutionEnd = executionTime != null ? (DateTime?)executionTime.Value.End : null;
            campaign.FeedbackStart = feedbackTime != null ? (DateTime?)feedbackTime.Value.Start : null;
            campaign.FeedbackEnd = feedbackTime != null ? (DateTime?)feedbackTime.Value.End : null;
            campaign.AmountMax = AmountMax;
            campaign.AmountMin = AmountMin;

            return campaign;
        }
    }

    public class CreateCampaignInfoViewModel
    {


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên chiến dịch")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Yêu cầu {0}")]
        [Display(Name = "Giới thiệu ngắn gọn sản phẩm, dịch vụ chạy chiến dịch")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "Yêu cầu {0}")]
        [Display(Name = "Liên kết URL")]
        public string Data { get; set; }

        [Display(Name = "Hình ảnh dùng làm Avatar")]
        public string Image { get; set; } = string.Empty;


        [Display(Name = "Hình ảnh đính kèm")]
        public List<string> AddonImages { get; set; } = new List<string>();

        [Required(ErrorMessage = "Hãy nhập dữ liệu {0}")]
        [Display(Name = "Yêu cầu cụ thể chiến dịch")]
        public string Requirement { get; set; }

        [Display(Name = "Thời gian")] //thời gian yêu cầu để avatar
        [Required(ErrorMessage = "Hãy nhập dữ liệu {0}")]
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

        public string ReviewDate { get; set; }
        public string ReviewAddress { get; set; }
        public string ReviewAddress2 { get; set; }
        public CampaignReviewType? ReviewType { get; set; }
        [Display(Name = "Thu hồi sản phẩm sau khi người dùng trải nghiệm")]
        public int? ReviewPayback { get; set; }
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


        [Range(10000, 100000000, ErrorMessage = "Chi phí tối thiểu phải lớn hơn 1.000đ")]
        [Display(Name = "Tối thiểu (Vnđ)")]
        public int AmountMin { get; set; } = 10000;

        [Range(10000, 100000000, ErrorMessage = "Chi phí phải lớn hơn 1.000đ")]

        [GreaterThan("AmountMin", ErrorMessage = "Chi phí tối đa phải lớn hơn chi phí tối thiểu")]
        [Display(Name = "Tối đa (Vnđ)")]
        public int AmountMax { get; set; } = 100000;

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




        [Display(Name = "KPIs(Like + Share + Comments) tối thiểu mà influencer sẽ phải đạt cho mỗi post để được ghi nhận doanh thu:")]
        public int KPIMin { get; set; }

        //[GreaterThan("KPIMin", ErrorMessage = "Cam kết lượng tương tác được tối thiểu phải lớn hơn hoặc bằng KPIMin")]
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

        [Display(Name = "Thời gian gửi mẫu nội dung")]
        public string FeedbackBefore { get; set; }


        [Display(Name = "Kols mà bạn muốn hợp tác")]
        public List<string> CustomKolNames { get; set; }

        public List<int> AccountIds { get; set; } = new List<int>();
        public List<int> AccountChargeAmounts { get; set; }


        [Display(Name = "Giới tính")]
        public int? ChildType { get; set; }

        [Display(Name = "Độ tuổi")]
        public int? ChildAgeMin { get; set; }
        public int? ChildAgeMax { get; set; }

    }

    public class GreaterThanAttribute : ValidationAttribute
    {

        public GreaterThanAttribute(string otherProperty)
            : base("{0} must be greater than {1}")
        {
            OtherProperty = otherProperty;
        }

        public string OtherProperty { get; set; }

        public string FormatErrorMessage(string name, string otherName)
        {
            return string.Format(ErrorMessageString, name, otherName);
        }

        protected override ValidationResult
            IsValid(object firstValue, ValidationContext validationContext)
        {
            var firstComparable = firstValue as IComparable;
            var secondComparable = GetSecondComparable(validationContext);

            if (firstComparable != null && secondComparable != null)
            {
                if (firstComparable.CompareTo(secondComparable) < 1)
                {
                    object obj = validationContext.ObjectInstance;
                    var thing = obj.GetType().GetProperty(OtherProperty);
                    var displayName = (DisplayAttribute)Attribute.GetCustomAttribute(thing, typeof(DisplayAttribute));

                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName, displayName.GetName()));
                }
            }

            return ValidationResult.Success;
        }

        protected IComparable GetSecondComparable(
            ValidationContext validationContext)
        {
            var propertyInfo = validationContext
                                  .ObjectType
                                  .GetProperty(OtherProperty);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(
                    validationContext.ObjectInstance, null);
                return secondValue as IComparable;
            }
            return null;
        }
    }
}
