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

        public CampaignType Type { get; set; }
        public int ServiceChargeAmount { get; set; }
        public int AccountChargeAmount { get; set; }
        public int AccountChargeExtraPercent { get; set; }
        public bool EnabledAccountChargeExtra { get; set; }
        public int AccountChargeTime { get; set; } //default = 1 --> Số lần thực hiện --> Nếu = kiểu = thay avatar thì mới lên số lần

        public CampaignStatus Status { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int Quantity { get; set; }

        private List<CampaignOption> _CampaignOption = new List<CampaignOption>();
        public IEnumerable<CampaignOption> CampaignOption => _CampaignOption.AsReadOnly();

        private List<CampaignAccount> _CampaignAccount = new List<CampaignAccount>();
        public IEnumerable<CampaignAccount> CampaignAccount => _CampaignAccount.AsReadOnly();


        private List<CampaignAccountType> _CampaignAccountType = new List<CampaignAccountType>();
        public IEnumerable<CampaignAccountType> CampaignAccountType => _CampaignAccountType.AsReadOnly();

    }

    public enum CampaignStatus
    {
        [DisplayName("Khởi tạo")]
        Created = 1,
        [DisplayName("Đã bắt đầu")]
        Started = 2,
        [DisplayName("Đã kết thúc")]
        Ended = 3,
        [DisplayName("Đã hoàn thành")]
        Completed = 4,
        [DisplayName("Đã Hủy")]
        Canceled = 5,
        [DisplayName("Lỗi")]
        Error = 6,
    }
}
