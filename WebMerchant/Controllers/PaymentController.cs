using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServices.Interfaces;
using WebServices.ViewModels;
using Common.Extensions;

namespace WebMerchant.Controllers
{

    public class PaymentController : BaseController
    {
        private readonly IWalletService _walletService;
        private readonly ICampaignService _campaignService;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;


        public PaymentController(IWalletService walletService, ICampaignService campaignService, 
            IPaymentService paymentService, INotificationService __INotificationService)
        {
            _campaignService = campaignService;
            _walletService = walletService;
            _paymentService = paymentService;
            _notificationService = __INotificationService;
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
            if (paymentResult.Status == Core.Entities.TransactionStatus.Completed && paymentResult.Amount > 0)
            {
                BackgroundJob.Enqueue<ICampaignService>(m => m.RequestJoinCampaignByAgency(CurrentUser.Id, model.CampaignId, CurrentUser.Username));

                //########### Longhk add create notification ##########################################################
                string _msg = string.Format("Chiến dịch \"{0}\" đã được thanh toán bởi doanh nghiệp \"{1}\", với số tiền {2} đ.", model.CampaignId, CurrentUser.Username, paymentResult.Amount.ToPriceText());
                string _data = "Campaign";
                await _notificationService.CreateNotification(model.CampaignId, EntityType.System, 0, NotificationType.AgencyPayCampaignService, _msg, _data);
                //#####################################################################################################
               
            }

            ViewBag.PaymentResult = paymentResult;
            return PartialView("ModalPaymentMessage");
        }


    }
}