using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Code;
using WebServices.Interfaces;

namespace WebServices.Jobs
{
    public class CampaignJob : ICampaignJob
    {
        private readonly ICampaignService _campaignService;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;
        private readonly IAccountService _accountService;
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
                        await _paymentService.CreatePaybackCampaignAccount(campaignid, accountid, SharedConstants.USERNAME);
                    }

                    //var isvalid = await _paymentService.VerifyPaybackCampaignAccount(campaignid);
                    //if (isvalid)
                    //{
                        await _campaignService.UpdateCampaignCompleted(campaignid, SharedConstants.USERNAME);

                        BackgroundJob.Enqueue<INotificationService>(m => m.CreateNotificationCampaignCompleted(campaignid));
                    //}
                    //else
                    //{
                    //    throw new Exception("Payback Invalid");
                    //}
                }
                catch (Exception ex)
                {
                    await _campaignService.UpdateCampaignError(campaignid, $"Lỗi khi hoàn thành chiến dịch: {ex.Message}", SharedConstants.USERNAME);
                }

            }

        }


        public async Task UpdateCampaignAccountExpired()
        {
            await _campaignService.UpdateCampaignAccountExpired();

        }

        public async Task UpdateCampaignProcess()
        {
            BackgroundJob.Enqueue<ICampaignJob>(m => m.UpdateCampaignStart());

            BackgroundJob.Enqueue<ICampaignJob>(m => m.UpdateCampaignEnd());

            //BackgroundJob.Schedule<ICampaignJob>(m => m.UpdateCompletedCampagin(0), TimeSpan.FromMinutes(2));
            BackgroundJob.Schedule<ICampaignJob>(m => m.UpdateCompletedCampagin(0), TimeSpan.FromMinutes(1));
        }
        public async Task UpdateCampaignStart()
        {
            await _campaignService.AutoUpdateStartedStatus(0);

        }

        public async Task UpdateCampaignEnd()
        {
            await _campaignService.AutoUpdateEndedStatus(0);

        }

        #region addition by longhk


        // longhk add
        public async Task CheckLockedCampagin()
        {
            await _campaignService.RunCheckingLockedStatus(0);
        }
        //####################################################################################################

        #endregion


    }
}
