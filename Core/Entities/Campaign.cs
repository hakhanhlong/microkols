using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Entities
{
    public class Campaign : BaseEntityWithMeta
    {
        public int AgencyId { get; set; }
        public Agency Agency { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        //Mô tả, Nội dung hoặc được link của campaign để người dùng thao tác....
        public string Data { get; set; }
        public string Image { get; set; }
   
        public string Requirement { get; set; }

        // Lay o settings
        public int ServiceChargePercent { get; set; }
        // Lay o settings
        public int ExtraOptionChargePercent { get; set; }

        public CampaignType Type { get; set; }
        public int ServicePrice { get; set; }
        public int AccountPrice { get; set; }
        public int AccountExtraPercent { get; set; }
        public bool EnabledAccountExtra { get; set; }

        public CampaignStatus Status { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

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
        Created,
        [DisplayName("Đã thanh toán")]
        Payed,
        [DisplayName("Đang xử lý")]
        Processing,
        [DisplayName("Đã bắt đầu")]
        Started,
        [DisplayName("Đã kết thúc")]
        Ended,
        [DisplayName("Hủy")]
        Canceled
    }
}
