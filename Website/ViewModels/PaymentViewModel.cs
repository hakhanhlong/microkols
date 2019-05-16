
using Common.Extensions;
using Core.Entities;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Website.Code;

namespace Website.ViewModels
{
    public class PaymentResultViewModel
    {
        public PaymentResultViewModel()
        {

        }
        public PaymentResultViewModel(PaymentResultErrorCode errorCode, int transactionid = 0, long amount = 0)
        {
            ErrorCode = errorCode;
            TransactionId = transactionid;
            Amount = amount;
            Status = TransactionStatus.Error;
        }
        public TransactionStatus Status { get; set; } = TransactionStatus.Error;
        public TransactionType Type { get; set; }
        public int TransactionId { get; set; }
        public long Amount { get; set; }
        public long SenderBalance { get; set; }
        public long ReceiverBalance { get; set; }
        public PaymentResultErrorCode ErrorCode { get; set; } = PaymentResultErrorCode.KhongLoi;
        public string ErrorMessage
        {
            get
            {
                return ErrorCode.ToDescription();
            }
        }
    }
    public enum PaymentResultErrorCode
    {
        [Description("")]
        KhongLoi = 0,
        [Description("Lỗi không xác định")]
        KhongXacDinh,
        [Description("Lỗi khi trừ tiền tài khoản")]
        TruTienLoi,
        [Description("Lỗi khi cộng tiền tài khoản")]
        CongTienLoi,
        [Description("Không đủ tiền để thực hiện giao dịch")]
        KhongDuTien,
        [Description("Thông tin tài khoản không chính xác")]
        ThongTinTaiKhoanKhongChinhXac ,
        [Description("Thông tin ví không chính xác")]
        ThongTinViKhongChinhXac,
        [Description("Thông tin thanh toán không chính xác")]
        ThongTinThanhToanKhongChinhXac,







    }



    #region Payment Campaign


    #endregion

}
