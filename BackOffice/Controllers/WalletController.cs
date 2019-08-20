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
        private readonly IAgencyBusiness _IAgencyBusiness;
        private readonly IAccountBusiness _IAccountBusiness;
        private readonly ITransactionBusiness _ITransactionBusiness;

        public WalletController(IWalletBusiness __IWalletBusiness, IAgencyBusiness __IAgencyBusiness,
            IAccountBusiness __IAccountBusiness, ITransactionBusiness __ITransactionBusiness)
        {
            _IWalletBusiness = __IWalletBusiness;
            _IAgencyBusiness = __IAgencyBusiness;
            _IAccountBusiness = __IAccountBusiness;
            _ITransactionBusiness = __ITransactionBusiness;
        }

        public IActionResult Index(int pageindex = 1)
        {
            var list_wallet = _IWalletBusiness.GetListWallet(pageindex, 20);


            if(list_wallet!= null)
            {
                foreach (var item in list_wallet.Wallets)
                {
                    if(item.EntityType == Core.Entities.EntityType.Account)
                    {
                        var account = _IAccountBusiness.GetAccount(item.EntityId);
                        if (account.Result != null)
                        {
                            item.Name = account.Result.Name;
                        }
                    }
                    else if(item.EntityType == Core.Entities.EntityType.Agency)
                    {
                        var agency = _IAgencyBusiness.GetAgency(item.EntityId);
                        if (agency.Result != null)
                        {
                            item.Name = agency.Result.Name;
                        }
                    }
                }
            }

            return View(list_wallet);            
        }

        public async Task<IActionResult> Transaction(int walletid, int pageindex = 1)
        {
            var list = await _ITransactionBusiness.GetTransactions(walletid, walletid, pageindex, 25);
            if (list == null)
            {
                TempData["MessageError"] = "Don't have transaction!";
            }
            return View(list);
        }


    }
}