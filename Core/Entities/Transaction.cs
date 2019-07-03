using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Entities
{
    public class Transaction : BaseEntityWithDate
    {
        public string Code { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public long Amount { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public string Data { get; set; }
        public int? RefId { get; set; }
        public string RefData { get; set; }
        public string Note { get; set; }
        public string AdminNote { get; set; }

        private List<TransactionHistory> _TransactionHistory = new List<TransactionHistory>();
        public IEnumerable<TransactionHistory> TransactionHistory => _TransactionHistory.AsReadOnly();
    }
    public enum TransactionStatus
    {
        All = -1,
        Created = 0,
        Canceled = 1,
        Processing = 2,
        Completed = 3,
        Error = 4
    }
    public enum TransactionType
    {
        [DisplayName("Không xác định")]
        Undefined = -1,
        [DisplayName("Nạp tiền")]
        WalletRecharge = 1,
        [DisplayName("Rút tiền")]
        WalletWithdraw = 2,
        [DisplayName("Phí dịch vụ")]
        CampaignServiceCharge = 3,
        [DisplayName("Phí thành viên")]
        CampaignAccountCharge = 4,
        [DisplayName("Chiến dịch trả thành viên")]
        CampaignAccountPayback = 5,
    }

}
