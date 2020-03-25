using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.DataAnnotations;

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
        public DateTime? CashoutDate { get; set; }
        public bool? IsCashOut { get; set; }

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
        [Display(Name ="Không xác định")]
        Undefined = -1,

        [Display(Name ="Nạp tiền")]
        WalletRecharge = 1,

        [Display(Name ="Rút tiền")]
        WalletWithdraw = 2,
        [Display(Name ="Phí dịch vụ")]
        CampaignServiceCharge = 3,
        [Display(Name ="Phí thành viên")]
        CampaignAccountCharge = 4,
        [Display(Name ="Bạn nhận được thanh toán")]
        CampaignAccountPayback = 5,
        [Display(Name ="Trừ tiền")]
        SubstractMoney = 6,
        [Display(Name ="Rút tiền thừa")]
        CampaignServiceCashBack = 7,
        [Display(Name ="Hoàn lại tiền Agency từ người dùng tham gia chiến dịch")]
        CampaignAccountRefundAgency = 8,
        [Display(Name ="Đã thực hiện chuyển tiền tới tài khoản ngân hàng của thành viên")] // tương ứng với việc trừ tiền trên ví của các thành viên này
        ExcecutedPaymentToAccountBanking = 9,
    }

}
