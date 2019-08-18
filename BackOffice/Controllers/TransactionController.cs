using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
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
        IPayoutExportRepository _IPayoutExportRepository;


        public TransactionController(ITransactionBusiness __ITransactionBusiness, IWalletBusiness __IWalletBusiness, IPayoutExportRepository __IPayoutExportRepository)
        {
            _ITransactionBussiness = __ITransactionBusiness;
            _IWalletBusiness = __IWalletBusiness;
            _IPayoutExportRepository = __IPayoutExportRepository;
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

            AccountType[] _accounttype;

            if (type == AccountType.All)
            {
                _accounttype = new AccountType[4];
                _accounttype[0] = AccountType.Regular;
                _accounttype[1] = AccountType.HotFacebooker;
                _accounttype[2] = AccountType.HotMom;
                _accounttype[3] = AccountType.HotTeen;
                //_accounttype[4] = AccountType.Kols;
            }
            else
            {
                _accounttype = new AccountType[1];
                _accounttype[0] = type;
            }

            var lastDateTime = DateTime.Now.AddMonths(-1);
            DateTime startDate = new DateTime(lastDateTime.Year, lastDateTime.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var PayoutExport = _IPayoutExportRepository.GetPayoutExport(startDate, endDate, type);

            var _listTransaction = await _ITransactionBussiness.GetPayoutTransactions(TransactionType.CampaignAccountPayback, TransactionStatus.Completed, _accounttype);

            ViewBag.PayoutExport = PayoutExport;
            return View(_listTransaction);
        }

        public async Task<IActionResult> AccountSubtractWallet(AccountType type = AccountType.All)
        {
            AccountType[] _accounttype;

            if (type == AccountType.All)
            {
                _accounttype = new AccountType[4];
                _accounttype[0] = AccountType.Regular;
                _accounttype[1] = AccountType.HotFacebooker;
                _accounttype[2] = AccountType.HotMom;
                _accounttype[3] = AccountType.HotTeen;
                //_accounttype[4] = AccountType.Kols;
            }
            else
            {
                _accounttype = new AccountType[1];
                _accounttype[0] = type;
            }

            var lastDateTime = DateTime.Now.AddMonths(-1);
            DateTime startDate = new DateTime(lastDateTime.Year, lastDateTime.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var PayoutExport = _IPayoutExportRepository.GetPayoutExport(startDate, endDate, type);

            var _listTransaction = await _ITransactionBussiness.GetPayoutTransactions(TransactionType.CampaignAccountPayback, TransactionStatus.Completed, _accounttype);
            if(_listTransaction.Count() > 0)
            {
                foreach(var transaction in _listTransaction)
                {
                    foreach(var item in transaction.Transactions)
                    {
                        int retValue = _ITransactionBussiness.UpdateCashOut(item.Id);
                        if (retValue > 0)
                        {
                            await _ITransactionBussiness.CalculateBalance(item.Id, item.Amount, item.ReceiverId, item.SenderId, "[Trả tiền mặt][AccountPayback]", HttpContext.User.Identity.Name);
                        }
                    }
                }

                if(PayoutExport != null)
                {
                    PayoutExport.IsUpdateWallet = true;
                    _IPayoutExportRepository.Update(PayoutExport);
                    TempData["MessageSuccess"] = "Substract Wallet CashOut Success!";
                }
            }

            return RedirectToAction("AccountPayback", "Transaction", new { type = type });

        }

        public async Task<IActionResult> ExportAccountPayback(AccountType type = AccountType.All)
        {
            AccountType[] _accounttype;

            if (type == AccountType.All)
            {
                _accounttype = new AccountType[4];
                _accounttype[0] = AccountType.Regular;
                _accounttype[1] = AccountType.HotFacebooker;
                _accounttype[2] = AccountType.HotMom;
                _accounttype[3] = AccountType.HotTeen;
                //_accounttype[4] = AccountType.Kols;
            }
            else
            {
                _accounttype = new AccountType[1];
                _accounttype[0] = type;
            }

            var _listTransaction = await _ITransactionBussiness.GetPayoutTransactions(TransactionType.CampaignAccountPayback, TransactionStatus.Completed,  _accounttype);
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


                //First add the headers
                worksheet.Cells[count_row_header, 1].Value = "STT";
                worksheet.Cells[count_row_header, 1].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 2].Value = "BANK ACCOUNT NAME";
                worksheet.Cells[count_row_header, 2].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 3].Value = "BANK NUMBER";
                worksheet.Cells[count_row_header, 3].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 4].Value = "BANK NAME";
                worksheet.Cells[count_row_header, 4].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 5].Value = "TOTAL";
                worksheet.Cells[count_row_header, 5].Style.Font.Bold = true;


                foreach (var transaction in _listTransaction)
                {

                    count_row_header++;

                    

                    worksheet.Cells[count_row_header, 1].Value = number_stt;
                    worksheet.Cells[count_row_header, 2].Value = transaction.Account.BankAccountName;
                    worksheet.Cells[count_row_header, 3].Value = transaction.Account.BankAccountNumber;
                    worksheet.Cells[count_row_header, 4].Value = transaction.Account.BankAccountBank;
                    worksheet.Cells[count_row_header, 5].Value = transaction.Transactions.Sum(s=>s.Amount).ToString();


                    number_stt++;
                }


                byte[] reportBytes = new byte[] { };

                var lastDateTime = DateTime.Now.AddMonths(-1);
                DateTime startDate = new DateTime(lastDateTime.Year, lastDateTime.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                try
                {
                    
                    reportBytes = package.GetAsByteArray();



                    bool isexist = _IPayoutExportRepository.IsExist(startDate, endDate, type);
                    if (!isexist)
                    {
                        _IPayoutExportRepository.Add(new PayoutExport() {
                            AccountType = type,
                            CreatedDate = DateTime.Now,
                            CreatedUser = HttpContext.User.Identity.Name,
                            IsExport = true,
                            IsUpdateWallet = false,
                            StartDateExport = startDate,
                            EndDateExport = endDate
                        });
                    }

                }
                catch { }

                return File(reportBytes, XlsxContentType, string.Format("{0}{1}{2}_{3}{4}{5}_{6}.xlsx", startDate.Day, startDate.Month, startDate.Year, endDate.Day, endDate.Month, endDate.Year, type.ToString()));



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