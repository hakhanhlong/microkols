using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;

namespace Website.Jobs
{
    public class CampaignJob
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


        }
    }
}
