using WebServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using WebServices.ViewModels;

namespace WebInfluencer.ViewComponents
{
    public class TransactionHistoryViewComponent : ViewComponent
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        public TransactionHistoryViewComponent(ITransactionService transactionService, IAccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string daterange, int page = 1, string vname = "Default")
        {
            ViewBag.DateRange = daterange;
            var currentUser = AuthViewModel.GetModel(HttpContext.User);
            ViewBag.Type = currentUser.Type;
            var model = await _transactionService.GetTransactionHistory(currentUser.Type,currentUser.Id, daterange, page, 20);
            return View(vname, model);
        }

    }
}
