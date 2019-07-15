using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{

    [Authorize]
    public class TransactionController : Controller
    {
        ITransactionBusiness _ITransactionBussiness;
        IWalletBusiness _IWalletBusiness;


        public TransactionController(ITransactionBusiness __ITransactionBusiness, IWalletBusiness __IWalletBusiness)
        {
            _ITransactionBussiness = __ITransactionBusiness;
            _IWalletBusiness = __IWalletBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> WalletRecharge(TransactionStatus status = TransactionStatus.All, int pageindex = 1)
        {

            var _listTransaction = await FillTransactions(TransactionType.WalletRecharge, status, pageindex);
            return View(_listTransaction);
        }

        public async Task<IActionResult> WalletWithdraw(TransactionStatus status = TransactionStatus.All, int pageindex = 1)
        {
            var _listTransaction = await FillTransactions(TransactionType.WalletWithdraw, status, pageindex);
            return View(_listTransaction);
        }

        public async Task<IActionResult> ServiceCharge(TransactionStatus status = TransactionStatus.All, int pageindex = 1)
        {
            var _listTransaction = await FillTransactions(TransactionType.CampaignServiceCharge, status, pageindex);
            return View(_listTransaction);
        }

        public async Task<IActionResult> AccountCharge(TransactionStatus status = TransactionStatus.All, int pageindex = 1)
        {
            var _listTransaction = await FillTransactions(TransactionType.CampaignAccountCharge, status, pageindex);
            return View(_listTransaction);
        }

        public async Task<IActionResult> AccountPayback(TransactionStatus status = TransactionStatus.All, int pageindex = 1)
        {
            var _listTransaction = await FillTransactions(TransactionType.CampaignAccountPayback, status, pageindex);
            return View(_listTransaction);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(TransactionStatus status, int id)
        {
            int code = await _ITransactionBussiness.UpdateStatus(status, id, HttpContext.User.Identity.Name);
            string _msg_code = "{\"Code\": -1, \"Message\": \"Update Status Error\"}";
            if(code == 9)
            {
                _msg_code = "{\"Code\": 1, \"Message\": \"Update Status Success\"}";

            }
            else if (code == 10){
                _msg_code = "{\"Code\": -1, \"Message\": \"wallet do not exist\"}";
            }
            else if (code == 11)
            {
                _msg_code = "{\"Code\": -1, \"Message\": \"wallet balance sender or receiver less then zero or amount could be abstract\"}";
            }

            return Json(_msg_code);
        }



        public async Task<ListTransactionViewModel> FillTransactions(TransactionType type, TransactionStatus status, int pageindex)
        {
            var _listTransactionViewModel = new ListTransactionViewModel();

            if (status == TransactionStatus.All)
            {
                _listTransactionViewModel = await _ITransactionBussiness.GetTransactionByType(type, pageindex, 20);
            }
            else
            {
                _listTransactionViewModel = await _ITransactionBussiness.GetTransactions(type, status, pageindex, 20);
            }


            foreach(var item in _listTransactionViewModel.Transactions)
            {
                try {
                    item.SenderName = _IWalletBusiness.Get(item.SenderId).Name;
                }
                catch { } 
                try {
                    item.ReceiverName = _IWalletBusiness.Get(item.ReceiverId).Name;
                }
                catch { }
            }


            return _listTransactionViewModel;
        }
        
    }
}