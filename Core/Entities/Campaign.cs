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
        public string Data { get; set; }
        public string Image { get; set; }
        public int CampaignTypeId { get; set; }
        public CampaignType CampaignType { get; set; }

        public int CampaignTypeCharge { get; set; }
        public int ServiceChargePercent { get; set; }
        public int ExtraChargePercent { get; set; }

        public CampaignStatus Status { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        private List<CampaignOption> _CampaignOption = new List<CampaignOption>();
        public IEnumerable<CampaignOption> CampaignOption => _CampaignOption.AsReadOnly();

        private List<CampaignAccount> _CampaignAccount = new List<CampaignAccount>();
        public IEnumerable<CampaignAccount> CampaignAccount => _CampaignAccount.AsReadOnly();

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
