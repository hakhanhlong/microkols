﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using BackOffice.Extensions;

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


        private void BuildFilterDataControl()
        {
            ViewBag.EntityTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Agency", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Account", Value = "1"},                
            };

            ViewBag.AccountTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "All", Value = "-1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Regular", Value = "0"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotTeen", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotMom", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotFacebooker", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Kols", Value = "4"},
            };
        }

        public IActionResult Index(int pageindex = 1)
        {
            var list_wallet = _IWalletBusiness.GetListWallet(pageindex, 20);
            BuildFilterDataControl();
            if (list_wallet!= null)
            {
                foreach (var item in list_wallet.Wallets)
                {
                    if(item.EntityType == Core.Entities.EntityType.Account)
                    {
                        var account = _IAccountBusiness.GetAccount(item.EntityId);
                        if (account.Result != null)
                        {
                            item.Name = account.Result.Name;
                            item.Type = account.Result.Type.ToString();
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

        public async Task<IActionResult> WalletAgency(int pageindex = 1)
        {
            var list_wallet = await _IWalletBusiness.GetListWallet(EntityType.Agency ,pageindex, 25);
            BuildFilterDataControl();
            if (list_wallet != null)
            {
                foreach (var item in list_wallet.Wallets)
                {
                    if (item.EntityType == Core.Entities.EntityType.Account)
                    {
                        var account = _IAccountBusiness.GetAccount(item.EntityId);
                        if (account.Result != null)
                        {
                            item.Name = account.Result.Name;
                            item.Type = account.Result.Type.ToString();
                        }
                    }
                    else if (item.EntityType == Core.Entities.EntityType.Agency)
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

        public async Task<IActionResult> WalletInfluencer(int pageindex = 1)
        {
            var list_wallet = await _IWalletBusiness.GetListWallet(EntityType.Account, pageindex, 25);
            BuildFilterDataControl();
            if (list_wallet != null)
            {
                foreach (var item in list_wallet.Wallets)
                {
                    if (item.EntityType == Core.Entities.EntityType.Account)
                    {
                        var account = _IAccountBusiness.GetAccount(item.EntityId);
                        if (account.Result != null)
                        {
                            item.Name = account.Result.Name;
                            item.Type = account.Result.Type.ToString();
                        }
                    }
                    else if (item.EntityType == Core.Entities.EntityType.Agency)
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






        public IActionResult Search(string keyword, EntityType EntityType, AccountType AccountType, int pageindex = 1)
        {            

            var list_wallet = _IWalletBusiness.Search(keyword, EntityType, AccountType, pageindex, 20);
            BuildFilterDataControl();
            if (list_wallet != null)
            {
                foreach (var item in list_wallet.Wallets)
                {
                    if (item.EntityType == Core.Entities.EntityType.Account)
                    {
                        var account = _IAccountBusiness.GetAccount(item.EntityId);
                        if (account.Result != null)
                        {
                            item.Name = account.Result.Name;
                            item.Type = account.Result.Type.ToString();
                        }
                    }
                    else if (item.EntityType == Core.Entities.EntityType.Agency)
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
            ViewBag.SearchTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Tất cả", Value = ""},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Cộng tiền", Value = "CongTien"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Trừ tiền", Value = "Trừ tiền"},
            };

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


        #region Transaction Agency

        public async Task<IActionResult> TransactionAgency(int walletid, int pageindex = 1)
        {
            ViewBag.SearchTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Tất cả", Value = ""},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = TransactionType.WalletRecharge.ToName() , Value = ((int)TransactionType.WalletRecharge).ToString()},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = TransactionType.CampaignServiceCharge.ToName() , Value = ((int)TransactionType.CampaignServiceCharge).ToString()},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = TransactionType.CampaignServiceCashBack.ToName() , Value = ((int)TransactionType.CampaignServiceCashBack).ToString()},
            };

            var list = await _ITransactionBusiness.GetTransactions(walletid, walletid, pageindex, 25);

            

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
                TempData["MessageError"] = "Don't have transaction!";
            }

            return View(list);
        }

        public IActionResult Statistic_TransactionAgency(int walletid = 0)
        {
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
            return View();
        }

        public async Task<IActionResult> TransactionAgencySearch(int walletid, TransactionType? SearchType, DateTime? StartDate, DateTime? EndDate, int pageindex = 1)
        {
            ViewBag.SearchTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Tất cả", Value = ""},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = TransactionType.WalletRecharge.ToName() , Value = ((int)TransactionType.WalletRecharge).ToString()},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = TransactionType.CampaignServiceCharge.ToName() , Value = ((int)TransactionType.CampaignServiceCharge).ToString()},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = TransactionType.CampaignServiceCashBack.ToName() , Value = ((int)TransactionType.CampaignServiceCashBack).ToString()},
            };

            ListTransactionViewModel list = null;

            list = await _ITransactionBusiness.GetTransactionsByType(SearchType, walletid, walletid, StartDate, EndDate, pageindex, 25);

            list.TransactionType = SearchType;

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
                TempData["MessageError"] = "Don't have transaction!";
            }

            return View(list);
        }



        #endregion




        #region Transaction Influencer

        public async Task<IActionResult> TransactionInfluencer(int walletid, int pageindex = 1)
        {
            ViewBag.SearchTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Tất cả", Value = ""},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = TransactionType.CampaignAccountPayback.ToName() , Value = ((int)TransactionType.CampaignAccountPayback).ToString()}
            };

            var list = await _ITransactionBusiness.GetTransactions(walletid, walletid, pageindex, 25);

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
                TempData["MessageError"] = "Don't have transaction!";
            }

            return View(list);
        }

        public IActionResult Statistic_TransactionInfluencer(int walletid = 0)
        {
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
            return View();
        }

        public async Task<IActionResult> TransactionInfluencerSearch(int walletid, TransactionType? SearchType, DateTime? StartDate, DateTime? EndDate, int pageindex = 1)
        {
            ViewBag.SearchTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Tất cả", Value = ""},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = TransactionType.CampaignAccountPayback.ToName() , Value = ((int)TransactionType.CampaignAccountPayback).ToString()}                
            };

            ListTransactionViewModel list = null;

            list = await _ITransactionBusiness.GetTransactionsByType(SearchType, walletid, walletid, StartDate, EndDate, pageindex, 25);

            list.TransactionType = SearchType;

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
                TempData["MessageError"] = "Don't have transaction!";
            }

            return View(list);
        }


        #endregion


        public async Task<IActionResult> TransactionSearch(int walletid, string SearchType, DateTime? StartDate, DateTime? EndDate, int pageindex = 1)
        {
            ViewBag.SearchTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Tất cả", Value = ""},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Cộng tiền", Value = "CongTien"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Trừ tiền", Value = "TruTien"},
            };

            ListTransactionViewModel list = null;

            list = await _ITransactionBusiness.GetTransactions(SearchType, walletid, walletid, StartDate, EndDate, pageindex, 25);

            list.SearchType = SearchType;

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
                        ViewBag.WalletFor = "NGƯỜI ẢNH HƯỞNG";
                    }
                }
                if (wallet.EntityType == Core.Entities.EntityType.Agency)
                {
                    var agency = _IAgencyBusiness.GetAgency(wallet.EntityId);
                    if (agency.Result != null)
                    {
                        ViewBag.Account = agency.Result;
                        ViewBag.WalletFor = "DOANH NGHIỆP";
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