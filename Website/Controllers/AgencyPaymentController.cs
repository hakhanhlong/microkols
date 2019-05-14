using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Interfaces;

namespace Website.Controllers
{
    [Authorize(Roles = "Agency")]
    public class AgencyPaymentController : BaseController
    {
        private readonly IWalletService _walletService;
        private readonly ICampaignService _campaignService;
        public AgencyPaymentController(IWalletService walletService, ICampaignService campaignService)
        {
            _campaignService = campaignService;
            _walletService = walletService;
        }

        public async Task<IActionResult> CampaignPayment(int campaignid)
        {
            var campaign = await _campaignService.GetCampaignDetailsByAgency(CurrentUser.Id, campaignid);
            ViewBag.Amount = await _walletService.GetAmount(CurrentUser);

            return PartialView();
        }
    }
}