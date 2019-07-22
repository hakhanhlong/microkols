using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace BackOffice.Controllers
{

    [Authorize]
    public class TransactionController : Controller
    {
        ITransactionBusiness _ITransactionBussiness;
        IWalletBusiness _IWalletBusiness;
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;


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

        public async Task<IActionResult> AccountPayback(AccountType type = AccountType.All)
        {
            //var _listTransaction = await FillTransactions(TransactionType.CampaignAccountPayback, status, pageindex);

            AccountType[] _accounttype = new AccountType[5];

            if (type == AccountType.All)
            {
                _accounttype[0] = AccountType.Regular;
                _accounttype[1] = AccountType.HotFacebooker;
                _accounttype[2] = AccountType.HotMom;
                _accounttype[3] = AccountType.HotTeen;
                _accounttype[4] = AccountType.Kols;
            }
            else
            {
                _accounttype[0] = type;
            }
                

            var _listTransaction = await _ITransactionBussiness.GetPayoutTransactions(TransactionType.CampaignAccountPayback, TransactionStatus.Completed, _accounttype);


            return View(_listTransaction);
        }

        public async Task<IActionResult> ExportAccountPayback(AccountType type = AccountType.All)
        {
            AccountType[] _accounttype = new AccountType[5];

            if (type == AccountType.All)
            {
                _accounttype[0] = AccountType.Regular;
                _accounttype[1] = AccountType.HotFacebooker;
                _accounttype[2] = AccountType.HotMom;
                _accounttype[3] = AccountType.HotTeen;
                _accounttype[4] = AccountType.Kols;
            }
            else
            {
                _accounttype[0] = type;
            }

            var _listTransaction = await _ITransactionBussiness.GetPayoutTransactions(TransactionType.CampaignAccountPayback, TransactionStatus.Completed,  _accounttype );
            if (_listTransaction != null)
            {

                var package = new ExcelPackage();
                package.Workbook.Properties.Title = "AccountPayback";
                package.Workbook.Properties.Author = "MicroKols.";
                package.Workbook.Properties.Subject = "Account Payback";
                package.Workbook.Properties.Keywords = "AccountPayback";


                var worksheet = package.Workbook.Worksheets.Add("Account Cash Out");

                int count_row_header = 1;
                int number_stt = 1;
                int turn = 1;
                foreach(var transaction in _listTransaction)
                {
                    //First add the headers
                    worksheet.Cells[count_row_header, 1].Value = "STT";
                    worksheet.Cells[count_row_header, 1].Style.Font.Bold = true;
                    worksheet.Cells[count_row_header, 2].Value = "DATETIME";
                    worksheet.Cells[count_row_header, 2].Style.Font.Bold = true;
                    worksheet.Cells[count_row_header, 3].Value = "AMOUNT";
                    worksheet.Cells[count_row_header, 3].Style.Font.Bold = true;

                    //Second add the headers                    
                    worksheet.Cells[(count_row_header + 1), 1].Value = "NAME";
                    worksheet.Cells[(count_row_header + 1), 1].Style.Font.Bold = true;
                    worksheet.Cells[(count_row_header + 1), 2].Value = transaction.Account.BankAccountName;
                    worksheet.Cells[(count_row_header + 1), 3].Value = "BANK NUMBER";
                    worksheet.Cells[(count_row_header + 1), 3].Style.Font.Bold = true;
                    worksheet.Cells[(count_row_header + 1), 4].Value = transaction.Account.BankAccountNumber;



                    int count = turn;
                    long total_amount = 0;
                    foreach(var item in transaction.Transactions)
                    {
                        //for the first item
                        if (count_row_header == 1)
                            count_row_header = 2;                        

                        worksheet.Cells[count_row_header + count, 1].Value = number_stt;                        
                        worksheet.Cells[count_row_header + count, 2].Value = item.DateModified.ToString("dd/MM/yyyy");                        
                        worksheet.Cells[count_row_header + count, 3].Value = item.Amount;

                        total_amount += item.Amount;
                        count++;
                        number_stt++;
                    }
                    if(count == transaction.Transactions.Count() + turn)
                    {                        
                        worksheet.Cells[count_row_header + count, 2].Value = "TOTAL";
                        worksheet.Cells[count_row_header + count, 2].Style.Font.Bold = true;
                        worksheet.Cells[count_row_header + count, 2].Style.Font.Color.SetColor(Color.Red);
                        worksheet.Cells[count_row_header + count, 3].Value = total_amount;
                        worksheet.Cells[count_row_header + count, 3].Style.Font.Bold = true;
                        worksheet.Cells[count_row_header + count, 3].Style.Font.Color.SetColor(Color.Red);

                    }

                    count_row_header += transaction.Transactions.Count() + 4;
                    turn++;

                }


                byte[] reportBytes = new byte[] { };
                try
                {
                    
                    reportBytes = package.GetAsByteArray();
                }
                catch { }

                return File(reportBytes, XlsxContentType, "report.xlsx");





                //worksheet.Cells[2, 1].Value = 1000;
                //worksheet.Cells[2, 2].Value = "Jon";
                //worksheet.Cells[2, 3].Value = "M";
                //worksheet.Cells[2, 4].Value = 5000;
                //worksheet.Cells[2, 4].Style.Numberformat.Format = numberformat;

                //worksheet.Cells[3, 1].Value = 1001;
                //worksheet.Cells[3, 2].Value = "Graham";
                //worksheet.Cells[3, 3].Value = "M";
                //worksheet.Cells[3, 4].Value = 10000;
                //worksheet.Cells[3, 4].Style.Numberformat.Format = numberformat;

                //worksheet.Cells[4, 1].Value = 1002;
                //worksheet.Cells[4, 2].Value = "Jenny";
                //worksheet.Cells[4, 3].Value = "F";
                //worksheet.Cells[4, 4].Value = 5000;
                //worksheet.Cells[4, 4].Style.Numberformat.Format = numberformat;





            }

            return RedirectToAction("AccountPayback", "Transaction", new { type = type });
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
                _msg_code = "{\"Code\": -1, \"Message\": \"Wallet do not exist\"}";
            }
            else if (code == 11)
            {
                _msg_code = "{\"Code\": -1, \"Message\": \"Wallet balance sender or receiver less then zero or amount could be abstract\"}";
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