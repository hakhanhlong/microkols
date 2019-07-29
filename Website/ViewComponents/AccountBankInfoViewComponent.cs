using Website.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Website.ViewModels;

namespace Website.ViewComponents
{
    public class AccountBankAccountViewComponent : ViewComponent
    {
        private readonly ICampaignService _campaignService;
        private readonly IAccountService _accountService;
        public AccountBankAccountViewComponent(ICampaignService campaignService, IAccountService accountService)
        {
            _campaignService = campaignService;
            _accountService = accountService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string vname = "Default")
        {
            var currentUser = AuthViewModel.GetModel(HttpContext.User);
            var model = await _accountService.GetBankAccount(currentUser.Id);
            return View(vname, model);
        }

    }
}
