using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebServices.Code;
using WebServices.Interfaces;
using WebServices.ViewModels;

namespace WebMerchant.Controllers
{

    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly ICampaignService _campaignService;
        public WalletController(IWalletService walletService, ICampaignService campaignService, ITransactionService transactionService, IAccountService accountService)
        {
            _transactionService = transactionService;
            _walletService = walletService;
            _accountService = accountService;
            _campaignService = campaignService;
        }

        public async Task<long> GetAmount()
        {
            return User.Identity.IsAuthenticated ? await _walletService.GetAmount(CurrentUser) : 0;
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.CountRecharge = await _transactionService.GetCount(CurrentUser.Id, Core.Entities.TransactionType.WalletRecharge);
            ViewBag.CountWithdraw = await _transactionService.GetCount(CurrentUser.Id, Core.Entities.TransactionType.WalletWithdraw);
            ViewBag.TotalRecharge = await _transactionService.GetTotalAmount(CurrentUser.Id, Core.Entities.TransactionType.WalletRecharge);
            ViewBag.TotalWithdraw = await _transactionService.GetTotalAmount(CurrentUser.Id, Core.Entities.TransactionType.WalletWithdraw);
            return View();
        }

        public async Task<IActionResult> History(TransactionType type,string daterange="", int pageindex = 1, int pagesize = 20)
        {
            ViewBag.Type = type;
            if (string.IsNullOrEmpty(daterange))
            {
                daterange = string.Format("{0} - {1}", new DateTime(2019, 1, 1).ToViDate(), DateTime.Now.ToViDate());
            }
            var model = await _transactionService.GetTransactionHistory(EntityType.Agency, CurrentUser.Id, type, daterange,pageindex, pagesize);

            ViewBag.DateRange = daterange;
            ViewBag.Total = await _transactionService.GetTotalAmount(CurrentUser.Id, type);
            return View(model);

        }
        #region Recharge

        public async Task<IActionResult> Recharge(int campaignid = 0)
        {
            long amount = 0;
            var note = "";
            if (campaignid > 0)
            {
                var campaign = await _campaignService.GetCampaignDetailsByAgency(CurrentUser.Id, campaignid);
                if (campaign != null)
                {
                    amount = campaign.Payment.TotalChargeAmount;
                    note = $"Nạp tiền chiến dịch {campaign.Code}";
                    ViewBag.Campaign = campaign;
                }

            }
            var model = new RechargeViewModel()
            {
                Amount = amount,
                Note = note,
                CampaignId = campaignid,

            };


            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Recharge(RechargeViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.CampaignId > 0)
                {
                    var campaign = await _campaignService.GetCampaignDetailsByAgency(CurrentUser.Id, model.CampaignId);
                    if (campaign != null)
                    {
                        model.Amount = campaign.Payment.TotalChargeAmount;
                        model.Note = $"Nạp tiền chiến dịch {campaign.Code}";

                        ViewBag.Campaign = campaign;
                    }
                }

                var r = await _transactionService.CreateTransaction(CurrentUser.Type, CurrentUser.Id, model, CurrentUser.Username);

                if (r > 0)
                {
                    ViewBag.RechargeModel = model;
                    this.AddAlertSuccess("Yêu cầu nạp tiền đã được gửi. Vui lòng chờ quản trị duyệt giao dịch.");

                }
                else if (r == -2)
                {
                    // var tid = await _transactionService.GetCurrentRechargeTransactionId(CurrentUser.Id);
                    ViewBag.Error = $"Bạn vui lòng chờ lệnh Nạp ví trước được duyệt để làm lệnh tiếp theo !";
                }
                else if (r == -3)
                {
                    this.AddAlertDanger($"Số tiền nhập {model.Amount} không chính xác");
                }
                else
                {
                    this.AddAlertDanger("Không tạo được yêu cầu nạp tiền. Vui lòng thử lại");
                }

            }
            else
            {
                this.AddAlertDanger("Thông tin nhập không chính xác");
            }

            return RedirectToAction("Recharge");
        }


        #endregion


        #region WithDraw
        public async Task<IActionResult> WithDraw()
        {
            ViewBag.Amount = await _walletService.GetAmount(CurrentUser);
            ViewBag.BankAccount = await _accountService.GetBankAccount(CurrentUser.Id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WithDraw(WithDrawViewModel model)
        {

            var amount = await _walletService.GetAmount(CurrentUser);
            if (ModelState.IsValid && model.Amount > 0 && model.Amount <= amount)
            {

                var r = await _transactionService.CreateTransaction(CurrentUser.Type, CurrentUser.Id, model, CurrentUser.Username);

                if (r > 0)
                {
                    this.AddAlertSuccess("Yêu cầu rút tiền đã được gửi. Vui lòng chờ quản trị duyệt giao dịch.");

                }
                else if (r == -2)
                {
                    this.AddAlertDanger("Đã tồn tại yêu cầu rút tiền với số tiền tương ứng. Vui lòng chờ duyệt giao dịch");
                }
                else
                {
                    this.AddAlertDanger("Không tạo được yêu cầu rút tiền. Vui lòng thử lại");
                }

            }
            else
            {
                this.AddAlertDanger("Thông tin rút tiền không chính xác");
            }

            return RedirectToAction("WithDraw");
        }





        #endregion
    }
}