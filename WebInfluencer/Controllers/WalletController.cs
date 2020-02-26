using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebServices.Code;
using WebServices.Interfaces;
using WebServices.ViewModels;

namespace WebInfluencer.Controllers
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

       
        public async Task<IActionResult> Index(string daterange, int pageindex = 1, int pagesize  = 20)
        {
            var model = await _transactionService.GetTransactionHistory(CurrentUser.Type, CurrentUser.Id, daterange, pageindex, 20);
            ViewBag.DateRange = daterange;
            return View(model);
        }

    }
}