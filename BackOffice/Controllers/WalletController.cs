using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {

        private readonly IWalletBusiness _IWalletBusiness;

        public WalletController(IWalletBusiness __IWalletBusiness)
        {
            _IWalletBusiness = __IWalletBusiness;
        }

        public IActionResult Index(int pageindex = 1)
        {
            var list_wallet = _IWalletBusiness.GetListWallet(pageindex, 20);
            return View(list_wallet);            
        }


    }
}