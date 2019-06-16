﻿using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;

namespace Website.Jobs
{
    public class CampaignJob : ICampaignJob
    {
        private readonly ICampaignService _campaignService;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;
        private readonly IAccountService _accountService;
        const string Username = "system";
        public CampaignJob(IPaymentService paymentService,
             IAccountService accountService,
            ICampaignService campaignService, INotificationService notificationService)
        {
            _campaignService = campaignService;
            _paymentService = paymentService;
            _notificationService = notificationService;
            _accountService = accountService;
        }
        public async Task UpdateCompletedCampagin(int campaignid = 0)
        {
            if (campaignid == 0)
            {
                var campaignids = await _campaignService.GetEndedCampaignIds();
                foreach (var id in campaignids)
                {
                    BackgroundJob.Enqueue<ICampaignJob>(m => m.UpdateCompletedCampagin(id));
                }
            }
            else
            {
                try
                {
                    var accountids = await _campaignService.GetFinishedAccountIdsByCampaignId(campaignid);

                    foreach (var accountid in accountids)
                    {
                        await _paymentService.CreatePaybackCampaignAccount(campaignid, accountid, Username);
                    }

                    var isvalid = await _paymentService.VerifyPaybackCampaignAccount(campaignid);
                    if (isvalid)
                    {
                        await _campaignService.UpdateCampaignCompleted(campaignid, Username);
                        BackgroundJob.Enqueue<INotificationService>(m => m.CreateNotificationCampaignCompleted(campaignid));
                    }
                    else
                    {
                        throw new Exception("Payback Invalid");

                    }
                }
                catch(Exception ex)
                {
                    await _campaignService.UpdateCampaignError(campaignid,$"Lỗi khi hoàn thành chiến dịch: {ex.Message}" , Username);
                }
                
            }

        }
    }
}
