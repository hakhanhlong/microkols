﻿using System;
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
        private readonly IAgencyService _IAgencyService;


        public PaymentController(IWalletService walletService, ICampaignService campaignService, 
            IPaymentService paymentService, INotificationService __INotificationService, IAgencyService ___IAgencyService)
        {
            _campaignService = campaignService;
            _walletService = walletService;
            _paymentService = paymentService;
            _notificationService = __INotificationService;
            _IAgencyService = ___IAgencyService;
        }

        public async Task<IActionResult> CampaignPayment(int campaignid)
        {
            var payment = await _campaignService.GetCampaignPaymentByAgency(CurrentUser.Id, campaignid);

            var list_pay_influencer = await _campaignService.GetCampaignDetailsByAgency(CurrentUser.Id, campaignid);

            ViewBag.Amount = await _walletService.GetAmount(CurrentUser);
            ViewBag.Payment = payment;

            if (payment.IsValid)
            {
                if (payment.TotalChargeAmount == 0)
                {
                    ViewBag.IsRutTienExist = await _paymentService.IsExistPaymentServiceCashBack(CurrentUser.Id, campaignid);
                }

                ViewBag.ListPayInfluencer = list_pay_influencer;
            }
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> CampaignPayment(CreateCampaignPaymentViewModel model)
        {
            var paymentResult = await _paymentService.CreateAgencyPayment(CurrentUser.Id, model, CurrentUser.Username);
            var campaign = await _campaignService.GetCampaignById(model.CampaignId);
            if (paymentResult.Status == Core.Entities.TransactionStatus.Completed && paymentResult.Amount > 0)
            {
               
                BackgroundJob.Enqueue<ICampaignService>(m => m.RequestJoinCampaignByAgency(CurrentUser.Id, model.CampaignId, CurrentUser.Username));
                //########### Longhk add create notification ##########################################################

                
                string _msg = string.Format("Chiến dịch \"{0}\" đã được thanh toán bởi doanh nghiệp \"{1}\", với số tiền {2}.", campaign.Title , CurrentUser.Name, paymentResult.Amount.ToPriceText());
                string _data = "Campaign";
                await _notificationService.CreateNotification(model.CampaignId, EntityType.System, 0, NotificationType.AgencyPayCampaignService, _msg, _data);
                //#####################################################################################################
               
            }
            
            if(paymentResult.ErrorCode == PaymentResultErrorCode.ChoHeThongDuyetRutTien)
            {
                try {
                    //########### Longhk add create notification ##########################################################
                    string _msg = string.Format("Doanh nghiệp \"{0}\" yêu cầu rút tiền chiến dịch \"{1}\", với số tiền {2}.", CurrentUser.Name, campaign.Title, paymentResult.Amount.ToPriceText());
                    string _data = "Transaction";
                    await _notificationService.CreateNotification(paymentResult.TransactionId, EntityType.System, 0, NotificationType.AgencyRequestWithdrawFromCampaign, _msg, _data);
                    //#####################################################################################################
                }
                catch { }                
            }

            ViewBag.PaymentResult = paymentResult;
            return PartialView("ModalPaymentMessage");
        }


    }
}