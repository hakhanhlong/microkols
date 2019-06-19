using Website.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Website.ViewComponents
{
    public class AccountLinkFacebookViewComponent : ViewComponent
    {
        private readonly ICampaignService _campaignService;
        private readonly IAccountService _accountService;
        public AccountLinkFacebookViewComponent(ICampaignService campaignService, IAccountService accountService)
        {
            _campaignService = campaignService;
            _accountService = accountService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int campaignid,string vname = "Default")
        {

            return View(vname);
        }

    }
}
