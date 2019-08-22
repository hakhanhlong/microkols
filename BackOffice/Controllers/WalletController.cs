using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {

        private readonly IWalletBusiness _IWalletBusiness;
        private readonly IWalletRepository _IWalletRepository;
        private readonly IAgencyBusiness _IAgencyBusiness;
        private readonly IAccountBusiness _IAccountBusiness;
        private readonly ITransactionBusiness _ITransactionBusiness;
        private readonly ITransactionHistoryBusiness _ITransactionHistoryBusiness;

        public WalletController(IWalletBusiness __IWalletBusiness, IAgencyBusiness __IAgencyBusiness,
            IAccountBusiness __IAccountBusiness, ITransactionBusiness __ITransactionBusiness, 
            IWalletRepository __IWalletRepository, ITransactionHistoryBusiness __ITransactionHistoryBusiness)
        {
            _IWalletBusiness = __IWalletBusiness;
            _IAgencyBusiness = __IAgencyBusiness;
            _IAccountBusiness = __IAccountBusiness;
            _ITransactionBusiness = __ITransactionBusiness;
            _IWalletRepository = __IWalletRepository;
            _ITransactionHistoryBusiness = __ITransactionHistoryBusiness;
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

            var wallet = _IWalletRepository.GetById(walletid);
            if (wallet != null)
            {
                ViewBag.Wallet = wallet;
                if(wallet.EntityType == Core.Entities.EntityType.Account) {
                    var account = _IAccountBusiness.GetAccount(wallet.EntityId);
                    if (account.Result != null)
                    {
                        ViewBag.Account = account.Result;
                    }
                }
                if (wallet.EntityType == Core.Entities.EntityType.Agency) {
                    var agency = _IAgencyBusiness.GetAgency(wallet.EntityId);
                    if (agency.Result != null)
                    {
                        ViewBag.Account = agency.Result;
                    }
                }               
            }
            

            if (list == null)
            {
                TempData["MessageError"] = "Don't have transaction!";
            }
            return View(list);
        }

        public async Task<IActionResult> TransactionHistory(int walletid, int pageindex = 1)
        {
            var list = await _ITransactionHistoryBusiness.GetByWalletID(walletid, pageindex);
            var wallet = _IWalletRepository.GetById(walletid);
            if (wallet != null)
            {
                ViewBag.Wallet = wallet;
                if (wallet.EntityType == Core.Entities.EntityType.Account)
                {
                    var account = _IAccountBusiness.GetAccount(wallet.EntityId);
                    if (account.Result != null)
                    {
                        ViewBag.Account = account.Result;
                    }
                }
                if (wallet.EntityType == Core.Entities.EntityType.Agency)
                {
                    var agency = _IAgencyBusiness.GetAgency(wallet.EntityId);
                    if (agency.Result != null)
                    {
                        ViewBag.Account = agency.Result;
                    }
                }
            }


            if (list == null)
            {
                TempData["MessageError"] = "Don't have transaction history!";
            }
            return View(list);
        }



    }
}