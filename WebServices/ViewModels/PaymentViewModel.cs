
using Common.Extensions;
using Core.Entities;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Code;

namespace WebServices.ViewModels
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
            Status = errorCode == PaymentResultErrorCode.ChoHeThongDuyetRutTien ? TransactionStatus.Processing : TransactionStatus.Error;
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
        [Display(Description = "")]
        KhongLoi = 0,
        [Display(Description = "Lỗi không xác định")]
        KhongXacDinh,
        [Display(Description = "Lỗi khi trừ tiền tài khoản")]
        TruTienLoi,
        [Display(Description = "Lỗi khi cộng tiền tài khoản")]
        CongTienLoi,
        [Display(Description = "Không đủ tiền để thực hiện giao dịch")]
        KhongDuTien,
        [Display(Description = "Thông tin tài khoản không chính xác")]
        ThongTinTaiKhoanKhongChinhXac ,
        [Display(Description = "Thông tin ví không chính xác")]
        ThongTinViKhongChinhXac,
        [Display(Description = "Thông tin thanh toán không chính xác")]
        ThongTinThanhToanKhongChinhXac,
        [Display(Description="Bạn đã khởi tạo yêu cầu rút tiền thành công. Vui lòng chờ quản trị duyệt yêu cầu")]
        ChoHeThongDuyetRutTien,







    }



    #region Payment Campaign


    #endregion

}
