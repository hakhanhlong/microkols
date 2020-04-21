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


    public class TransactionStatistic
    {
        public string Timeline { get; set; }
        public TransactionType Type { get; set; }
        public long Amount { get; set; }

    }

    public class TransactionCampaignRevenue
    {
        public long TotalCampaignServiceCharge { get; set; }
        public long TotalCampaignServiceCashback { get; set; }
        public long TotalCampaignAccountPayback { get; set; }
        public long TotalCampaignRevenue { get; set; }

    }


    public enum TransactionStatus
    {
        [Display(Name = "Không xác định")]
        All = -1,

        [Display(Name = "Khởi tạo")]
        Created = 0,

        [Display(Name = "Hủy bỏ")]
        Canceled = 1,

        [Display(Name = "Đang xử lý")]
        Processing = 2,

        [Display(Name = "Thành công")]
        Completed = 3,

        [Display(Name = "Lỗi, Không thành công")]
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
        [Display(Name ="Phí dịch vụ chiến dịch")]
        CampaignServiceCharge = 3,
        [Display(Name ="Phí thành viên")]
        CampaignAccountCharge = 4,
        [Display(Name ="Influencer nhận thanh toán về ví")]
        CampaignAccountPayback = 5,
        [Display(Name ="Trừ tiền")]
        SubstractMoney = 6,
        [Display(Name ="Rút tiền thừa chiến dịch")]
        CampaignServiceCashBack = 7,
        [Display(Name ="Hoàn lại tiền Agency từ người dùng tham gia chiến dịch")]
        CampaignAccountRefundAgency = 8,
        [Display(Name ="Chuyển lợi nhuận tới tài khoản ngân hàng Influencer")] // tương ứng với việc trừ tiền trên ví của các thành viên này
        ExcecutedPaymentToAccountBanking = 9
    }

}
