using System;
using System.Collections.Generic;
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
        public string Note { get; set; }

        private List<TransactionHistory> _TransactionHistory = new List<TransactionHistory>();
        public IEnumerable<TransactionHistory> TransactionHistory => _TransactionHistory.AsReadOnly();
    }
    public enum TransactionStatus
    {
        Created,
        Processing,
        Canceled,
        Completed,
    }
    public enum TransactionType
    {
        Undefined = -1,
        WalletRecharge = 1,
        WalletWithdraw = 2,
        CampaignServiceCharge = 3,
        CampaignAccountCharge = 4,
    }

}
