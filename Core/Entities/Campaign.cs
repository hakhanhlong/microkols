using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }


        public string CustomKolNames { get; set; }
        public DateTime? AccountFeedbackBefore { get; set; }

        public int Quantity { get; set; }

        private List<CampaignOption> _CampaignOption = new List<CampaignOption>();
        public IEnumerable<CampaignOption> CampaignOption => _CampaignOption.AsReadOnly();

        private List<CampaignAccount> _CampaignAccount = new List<CampaignAccount>();
        public IEnumerable<CampaignAccount> CampaignAccount => _CampaignAccount.AsReadOnly();


        private List<CampaignAccountType> _CampaignAccountType = new List<CampaignAccountType>();
        public IEnumerable<CampaignAccountType> CampaignAccountType => _CampaignAccountType.AsReadOnly();

    }
    public enum CampaignMethod
    {
        [DisplayName("Tự chọn thành viên")]
        ChooseAccount = 0,
        [DisplayName("Thành viên tự đăng ký")]
        OpenJoined = 1
    }

    public enum CampaignStatus
    {
        [DisplayName("Chờ phê duyệt")]
        Created = 0,
        [DisplayName("Đã phê duyệt")]
        Confirmed = 1,
        [DisplayName("Bắt đầu")]
        Started = 2,
        [DisplayName("Kết thúc")]
        Ended = 3,
        [DisplayName("Hoàn thành")]
        Completed = 4,
        [DisplayName("Chiến dịch đã bị hủy")]
        Canceled = 5,
        [DisplayName("Lỗi")]
        Error = 6,

        //[DisplayName("Chờ phê duyệt")]
        //ChoPheDuyet = 0,
        //[DisplayName("Bắt đầu")]
        //BatDau = 1,
        //[DisplayName("Thực hiện")]
        //ThucHien = 2,
        //[DisplayName("Theo dõi")]
        //TheoDoi = 3,
        //[DisplayName("Hoàn thành")]
        //HoanThanh = 4,
        //[DisplayName("Kết thúc")]
        //KetThuc = 5,
        //[DisplayName("Đã Hủy")]
        //DaHuy = 5,
        //[DisplayName("Lỗi")]
        //Loi = 6,
    }




}
