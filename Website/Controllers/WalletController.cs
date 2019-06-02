using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Website.Code;
using Website.Interfaces;
using Website.ViewModels;

namespace Website.Controllers
{
    [Authorize]
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService) 
        {
            _walletService = walletService;
        }

        public async Task<long> GetAmount()
        {            
            return User.Identity.IsAuthenticated ? await _walletService.GetAmount(CurrentUser) : 0;
        }



        #region Recharge
        public IActionResult Recharge()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Recharge(RechargeViewModel model)
        {
            if (ModelState.IsValid)
            {
                /*
                var r = await _transactionService.CreateTransaction(CurrentUser.Id, model, CurrentUser.Username);

                if (r > 0)
                {
                    ViewBag.RechargeModel = model;
                    return PartialView("RechargeSuccess");
                }
                else if (r == -2)
                {
                    var tid = await _transactionService.GetCurrentRechargeTransactionId(CurrentUser.Id);
                    ViewBag.Error = $"Bạn vui lòng chờ lệnh Nạp ví có mã giao dịch #{tid} được duyệt để làm lệnh tiếp theo !";
                }
                else if (r == -3)
                {
                    ViewBag.Error = $"Số tiền nhập {model.Amount} không chính xác";
                }
                else
                {
                    ViewBag.Error = "Không tạo được yêu cầu nạp tiền. Vui lòng thử lại";
                }
                */
            }
            else
            {
                ViewBag.Error = "Thông tin nhập không chính xác";
            }

            return PartialView("RechargeError");
        }


        #endregion


        #region WithDraw
        public async Task<IActionResult> WithDraw()
        {
            ViewBag.Amount = await _walletService.GetAmount(CurrentUser);
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> WithDraw(WithDrawViewModel model)
        {

            var amount = await _walletService.GetAmount(CurrentUser);
            if (ModelState.IsValid && model.Amount > 0 && model.Amount <= amount)
            {
                /*
                var r = await _transactionService.CreateTransaction(CurrentUser.Id, model, CurrentUser.Username);

                if (r > 0)
                {
                    ViewBag.Success = "Yêu cầu rút tiền đã được gửi. Vui lòng chờ 3 ngày làm việc để nhận được tiền trong tài khoản bạn yêu cầu.";
                    return PartialView("WithDrawMessage");
                }
                else if (r == -2)
                {
                    ViewBag.Error = "Đã tồn tại yêu cầu rút tiền với số tiền tương ứng. Vui lòng chờ duyệt giao dịch";
                }
                else
                {
                    ViewBag.Error = "Không tạo được yêu cầu rút tiền. Vui lòng thử lại";
                }
                */
            }
            else
            {
                ViewBag.Error = "Thông tin rút tiền không chính xác";
            }

            return PartialView("WithDrawMessage");
        }



     

        #endregion
    }
}