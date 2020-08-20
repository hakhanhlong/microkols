using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Campaign : BaseEntityWithMeta
    {

        public string Code { get; set; }
        public int AgencyId { get; set; }
        public Agency Agency { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        //Mô tả, Nội dung hoặc được link của campaign để người dùng thao tác....
        public string Data { get; set; }
        public string Image { get; set; }

        public string Requirement { get; set; }

        public string SystemNote { get; set; }
        // Lay o settings
        public int ServiceChargePercent { get; set; }
        // Lay o settings
        public int ExtraOptionChargePercent { get; set; }
        // Lay o settings
        public int? ServiceVATPercent { get; set; }

        public CampaignType Type { get; set; }
        public CampaignMethod? Method { get; set; }
        public string Hashtag { get; set; }
        public string SampleContent { get; set; }
        public string SampleContentText { get; set; }


        // các trường này ko dùng trong cách tính mới nữa
        public int ServiceChargeAmount { get; set; }
        public int AccountChargeAmount { get; set; }
        public int AccountChargeExtraPercent { get; set; }
        public bool EnabledAccountChargeExtra { get; set; }
        public int AccountChargeTime { get; set; } //default = 1 --> Số lần thực hiện --> Nếu = kiểu = thay avatar thì mới lên số lần
        //end cac truong ko dung


        public CampaignStatus Status { get; set; }

        public bool? IsSendProduct { get; set; }


        public int? KPIMin { get; set; }
        public int? InteractiveMin { get; set; }


        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public DateTime? ExecutionStart { get; set; }
        public DateTime? ExecutionEnd { get; set; }
        public DateTime? AccountFeedbackBefore { get; set; }
        public DateTime? FeedbackStart { get; set; }
        public DateTime? FeedbackEnd { get; set; }

        public DateTime? ReviewStart { get; set; }
        public DateTime? ReviewEnd { get; set; }

        public string ReviewAddress { get; set; }

        public CampaignReviewType? ReviewType { get; set; }
        public bool? ReviewPayback { get; set; }

        public int AmountMin { get; set; }
        public int AmountMax { get; set; }
        public int Quantity { get; set; }

        public string CustomKolNames { get; set; }

        private List<CampaignOption> _CampaignOption = new List<CampaignOption>();
        public IEnumerable<CampaignOption> CampaignOption => _CampaignOption.AsReadOnly();

        private List<CampaignAccount> _CampaignAccount = new List<CampaignAccount>();
        public IEnumerable<CampaignAccount> CampaignAccount => _CampaignAccount.AsReadOnly();


        private List<CampaignAccountType> _CampaignAccountType = new List<CampaignAccountType>();
        public IEnumerable<CampaignAccountType> CampaignAccountType => _CampaignAccountType.AsReadOnly();

        public string HrefCompare { get; set; }

    }

    public enum CampaignReviewType
    {
        [Display(Name = "Gửi sản phẩm trải nghiệm đến địa chỉ của Influencers", Description ="Nhập địa chỉ nhận lại sản phẩm sau khi người dùng trải nghiệm")]
        GuiSanPham = 0,
        [Display(Name = "Mời người dùng đến trải nghiệm", Description = "Nhập địa chỉ trải nghiệm sản phẩm")]
        MoiNguoiDung = 1
    }
    public enum CampaignMethod
    {
        [Display(Name ="Tự chọn thành viên")]
        ChooseAccount = 0,
        [Display(Name ="Thành viên tự đăng ký")]
        OpenJoined = 1
    }

    
    [Flags]
    public enum CampaignStatus
    {
        [Display(Name ="Chờ phê duyệt")]
        Created = 0,
        [Display(Name ="Đã phê duyệt")]
        Confirmed = 1,
        [Display(Name ="Bắt đầu")]
        Started = 2,
        [Display(Name ="Ended")]
        Ended = 3,
        [Display(Name = "Kết thúc")] //Hoàn thành
        Completed = 4,
        [Display(Name ="Chiến dịch đã bị hủy")]
        Canceled = 5,
        [Display(Name ="Lỗi")]
        Error = 6,
        [Display(Name = "Tạm khóa & Cần thanh toán")]
        Locked = 7,
        //[Display(Name = "Đã thanh toán")]
        //Paid = 8,
        //[Display(Name = "Đã phê duyệt & Thanh toán đủ")]
        //ConfirmedAndPaid = Confirmed & Paid,


        //[Display(Name ="Chờ phê duyệt")]
        //ChoPheDuyet = 0,
        //[Display(Name ="Bắt đầu")]
        //BatDau = 1,
        //[Display(Name ="Thực hiện")]
        //ThucHien = 2,
        //[Display(Name ="Theo dõi")]
        //TheoDoi = 3,
        //[Display(Name ="Hoàn thành")]
        //HoanThanh = 4,
        //[Display(Name ="Kết thúc")]
        //KetThuc = 5,
        //[Display(Name ="Đã Hủy")]
        //DaHuy = 5,
        //[Display(Name ="Lỗi")]
        //Loi = 6,
    }




}
