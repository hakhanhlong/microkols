using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Interfaces;
using Website.ViewModels;
namespace Website.Controllers
{
    [Authorize(Roles = "Agency")]
    public class AgencyPaymentController : BaseController
    {
        private readonly IWalletService _walletService;
        private readonly ICampaignService _campaignService;
        private readonly IPaymentService _paymentService;
      
        public AgencyPaymentController(IWalletService walletService, ICampaignService campaignService, IPaymentService paymentService)
        {
            _campaignService = campaignService;
            _walletService = walletService;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> CampaignPayment(int campaignid)
        {
            var payment = await _campaignService.GetCampaignPaymentByAgency(CurrentUser.Id, campaignid);
            ViewBag.Amount = await _walletService.GetAmount(CurrentUser);
            ViewBag.Payment = payment;
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> CampaignPayment(CreateCampaignPaymentViewModel model)
        {
            var paymentResult = await _paymentService.CreateAgencyPayment(CurrentUser.Id, model, CurrentUser.Username);
            ViewBag.PaymentResult = paymentResult;
            if (paymentResult.Status == Core.Entities.TransactionStatus.Completed)
            {
             
                var accountids = new List<int>();

                for (var i = 0; i < accountids.Count; i++)
                {
                    BackgroundJob.Enqueue<ICampaignService>(m => m.RequestJoinCampaignByAgency(CurrentUser.Id, model.CampaignId, accountids[i],  CurrentUser.Username));

                }
            }
            return PartialView("ModalPaymentMessage");
        }


    }
}