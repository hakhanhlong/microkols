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
using WebServices.Interfaces;

namespace BackOffice.Controllers
{

    [Authorize]
    public class TransactionController : Controller
    {
        ITransactionBusiness _ITransactionBussiness;
        ITransactionService _ITransactionService;

        ITransactionRepository _ITransactionRepository;

        IWalletBusiness _IWalletBusiness;
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;
        IPayoutExportRepository _IPayoutExportRepository;
        private readonly INotificationBusiness _INotificationBusiness;
        private readonly ITransactionHistoryBusiness _ITransactionHistoryBusiness;


        public TransactionController(ITransactionBusiness __ITransactionBusiness, IWalletBusiness __IWalletBusiness, 
            IPayoutExportRepository __IPayoutExportRepository, ITransactionRepository __ITransactionRepository, INotificationBusiness __INotificationBusiness, 
            ITransactionHistoryBusiness __ITransactionHistoryBusiness, ITransactionService __ITransactionService)
        {
            _ITransactionBussiness = __ITransactionBusiness;
            _IWalletBusiness = __IWalletBusiness;
            _IPayoutExportRepository = __IPayoutExportRepository;
            _ITransactionRepository = __ITransactionRepository;
            _INotificationBusiness = __INotificationBusiness;
            _ITransactionHistoryBusiness = __ITransactionHistoryBusiness;
            _ITransactionService = __ITransactionService;
        }

        public IActionResult Index()
        {
            return View();
        }


        private void BindingTransactionOptions()
        {
            ViewBag.TransactionStatus = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "All", Value = "-1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Created", Value = "0"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Canceled", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Processing", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Completed", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Error", Value = "4"},
            };
        
        }


        #region CampaignServiceCashBack

        public async Task<IActionResult> CampaignServiceCashBack(TransactionStatus status = TransactionStatus.All, int pageindex = 1)
        {
            BindingTransactionOptions();
            var _listTransaction = await FillTransactions(TransactionType.CampaignServiceCashBack, status, pageindex);
            return View(_listTransaction);
        }

        public async Task<IActionResult> CampaignServiceCashBackSearch(string keyword, DateTime? StartDate, DateTime? EndDate, TransactionStatus TransactionStatus = TransactionStatus.All, int pageindex = 1)
        {
            BindingTransactionOptions();


            var _listTransaction = await _ITransactionBussiness.TransactionAgencyCampaignServiceCashBackSearch(keyword, TransactionStatus, StartDate, EndDate, pageindex, 25);

            foreach (var item in _listTransaction.Transactions)
            {
                try
                {
                    if (item.SenderId == 1)
                    {
                        item.SenderName = "System";
                    }
                    else
                    {
                        var wallet = _IWalletBusiness.Get(item.SenderId);
                        item.SenderName = wallet.Name;
                        item.Wallet = wallet;
                    }

                }
                catch { }
                try
                {
                    var wallet = _IWalletBusiness.Get(item.ReceiverId);
                    item.ReceiverName = wallet.Name;
                    item.Wallet = wallet;
                }
                catch { }
            }

            return View(_listTransaction);
        }


        #endregion


        #region WalletRecharge


        public async Task<IActionResult> WalletRecharge(TransactionStatus status = TransactionStatus.All, int pageindex = 1)
        {

            BindingTransactionOptions();
            var _listTransaction = await FillTransactions(TransactionType.WalletRecharge, status, pageindex);
            return View(_listTransaction);
        }

        public async Task<IActionResult>  WalletRechargeSearch(string keyword, DateTime? StartDate, DateTime? EndDate, TransactionStatus TransactionStatus = TransactionStatus.All, int pageindex = 1)
        {
            BindingTransactionOptions();


            var _listTransaction = await _ITransactionBussiness.TransactionAgencyWalletRechargeSearch(keyword, TransactionStatus, StartDate, EndDate, pageindex, 25);

            foreach (var item in _listTransaction.Transactions)
            {
                try
                {
                    if (item.SenderId == 1)
                    {
                        item.SenderName = "System";
                    }
                    else
                    {
                        var wallet = _IWalletBusiness.Get(item.SenderId);
                        item.SenderName = wallet.Name;
                        item.Wallet = wallet;
                    }

                }
                catch { }
                try
                {
                    var wallet = _IWalletBusiness.Get(item.ReceiverId);
                    item.ReceiverName = wallet.Name;
                    item.Wallet = wallet;
                }
                catch { }
            }

            return View(_listTransaction);
        }

        #endregion


        public async Task<IActionResult> WalletWithdraw(TransactionStatus status = TransactionStatus.All, int pageindex = 1)
        {
            var _listTransaction = await FillTransactions(TransactionType.WalletWithdraw, status, pageindex);
            return View(_listTransaction);
        }


        #region CampaignServiceCharge
        public async Task<IActionResult> ServiceCharge(TransactionStatus status = TransactionStatus.All, int pageindex = 1)
        {
            BindingTransactionOptions();
            var _listTransaction = await FillTransactions(TransactionType.CampaignServiceCharge, status, pageindex);
            foreach (var item in _listTransaction.Transactions)
            {
                try
                {
                    if (item.SenderId == 1)
                    {
                        item.SenderName = "System";
                    }
                    else
                    {
                        var wallet = _IWalletBusiness.Get(item.SenderId);
                        item.SenderName = wallet.Name;
                        item.Wallet = wallet;
                    }

                }
                catch { }
                
            }

            return View(_listTransaction);
        }

        public async Task<IActionResult> CampaignServiceSearch(string keyword, DateTime? StartDate, DateTime? EndDate, TransactionStatus TransactionStatus = TransactionStatus.All, int pageindex = 1)
        {
            BindingTransactionOptions();

            var _listTransaction = await _ITransactionBussiness.TransactionAgencyCampaignServiceSearch(keyword, TransactionStatus, StartDate, EndDate, pageindex, 25);

            foreach (var item in _listTransaction.Transactions)
            {
                try
                {
                    if (item.SenderId == 1)
                    {
                        item.SenderName = "System";
                    }
                    else
                    {
                        var wallet = _IWalletBusiness.Get(item.SenderId);
                        item.SenderName = wallet.Name;
                        item.Wallet = wallet;
                    }

                }
                catch { }
                
            }

            return View(_listTransaction);
        }


        #endregion



        public async Task<IActionResult> AccountCharge(TransactionStatus status = TransactionStatus.All, int pageindex = 1)
        {
            var _listTransaction = await FillTransactions(TransactionType.CampaignAccountCharge, status, pageindex);
            return View(_listTransaction);
        }

        public async Task<IActionResult> History(int transactionid, int walletid)
        {
            var transactionhistories = await _ITransactionHistoryBusiness.GetByTransactionID(transactionid);

            return View(transactionhistories);
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
            int count_fail = 0;

            if(_listTransaction.Count() > 0)
            {
                foreach(var transaction in _listTransaction)
                {
                    foreach(var item in transaction.Transactions)
                    {


                        if(item.IsCashOut.HasValue == false || (item.IsCashOut.HasValue && item.IsCashOut.Value == false))
                        {
                            int senderid = item.ReceiverId;
                            int receiverid = item.SenderId;
                            long money_number = item.Amount;
                            int campaignid = item.RefId.Value;
                            int accountid = transaction.Account.Id;

                            string txt_note = string.Format("Hệ thống thực hiện trừ tiền {0} đ trên ví walletid = {1}, vì hệ thống đã chuyển {2} đ đến tài khoản ngân hàng của thành viên thuộc walletid={3}. Và cộng {4} đ vào ví walletid={5}",
                                money_number, senderid, money_number, senderid, money_number, receiverid);



                            int transactionid = await _ITransactionRepository.CreateTransaction(senderid, receiverid, money_number, TransactionType.ExcecutedPaymentToAccountBanking, txt_note, string.Format("Campaign ID = {0}", campaignid), HttpContext.User.Identity.Name, campaignid);

                            if (transactionid > 0) // nếu tạo transaction thành công
                            {
                                int retResult = await _ITransactionBussiness.CalculateBalance(transactionid, item.Amount, senderid, receiverid, "[Chuyển tiền đến tài khoản ngân hàng của thành viên][ExcecutedPaymentToAccountBanking]", HttpContext.User.Identity.Name);
                                /*
                                * 09: success
                                * 10: wallet do not exist
                                * 11: wallet balance sender or receiver less then zero or amount could be abstract
                                * 
                                */

                                try
                                {

                                    switch (retResult)
                                    {
                                        case 9:
                                            int retValue = _ITransactionBussiness.UpdateCashOut(item.Id);
                                            await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Completed, "Success", HttpContext.User.Identity.Name);//
                                            await _ITransactionRepository.UpdateTransactionStatus(item.Id, TransactionStatus.Completed, "Success", HttpContext.User.Identity.Name);// delete transaction if case error
                                            NotificationType notificationType = NotificationType.ExcecutedPaymentToAccountBanking;
                                            string msg = string.Format("Hệ thống đã chuyển tiền {0} đ tới tài khoản ngân hàng của bạn và tự động trừ tiền trong ví tương ứng với số tiền {1}, từ chiến dịch bạn đã tham gia", money_number, money_number);
                                            await _INotificationBusiness.CreateNotificationExcecutedPaymentToAccountBanking(campaignid, accountid, notificationType, msg, "");
                                            break;
                                        case 10:
                                            await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Error, "Wallet do not exist", HttpContext.User.Identity.Name);// delete transaction if case error
                                            await _ITransactionRepository.UpdateTransactionStatus(item.Id, TransactionStatus.Completed, "Wallet do not exist", HttpContext.User.Identity.Name);// delete transaction if case error
                                            count_fail++;
                                            break;
                                        case 11:
                                            await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Error, "Wallet balance sender or receiver less then zero or amount could be abstract", HttpContext.User.Identity.Name);// delete transaction if case error
                                            await _ITransactionRepository.UpdateTransactionStatus(item.Id, TransactionStatus.Completed, "Wallet balance sender or receiver less then zero or amount could be abstract", HttpContext.User.Identity.Name);// delete transaction if case error
                                            count_fail++;
                                            break;
                                        case 12:
                                            await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Error, "Wallet balance sender do not enought balance", HttpContext.User.Identity.Name);// delete transaction if case error
                                            await _ITransactionRepository.UpdateTransactionStatus(item.Id, TransactionStatus.Completed, "Wallet balance sender do not enought balance", HttpContext.User.Identity.Name);// delete transaction if case error
                                            count_fail++;
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    TempData["MessageError"] = ex.Message;
                                }

                            }
                        }
                        


                                                                   
                    }
                }

                if(count_fail == 0)
                {
                    if (PayoutExport != null)
                    {
                        PayoutExport.IsUpdateWallet = true;
                        _IPayoutExportRepository.Update(PayoutExport);
                        TempData["MessageSuccess"] = "Substract Wallet CashOut Success!";
                    }
                }

               
            }

            return RedirectToAction("AccountPayback", "Transaction", new { type = type });

        }

        public async Task<ActionResult> Detail(int id = 0)
        {

            var transaction = await _ITransactionBussiness.Get(id);

            try
            {
                if (transaction.SenderId == 1)
                {
                    transaction.SenderName = "System";
                }
                else
                {
                    var wallet = _IWalletBusiness.Get(transaction.SenderId);
                    transaction.SenderName = wallet.Name;
                    transaction.Wallet = wallet;
                }

            }
            catch { }
            try
            {
                var wallet = _IWalletBusiness.Get(transaction.ReceiverId);
                transaction.ReceiverName = wallet.Name;
                transaction.Wallet = wallet;
            }
            catch { }



            return View(transaction);

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



        //dùng cho nap tiền agency
        public async Task<IActionResult> TransactionUpdateStatus(int id, TransactionStatus status)
        {         
            ViewBag.TransactionStatus = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Canceled", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Processing", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Completed", Value = "3"}
            };
            var transaction = await _ITransactionRepository.GetByIdAsync(id);
            return View(new TransactionViewModel(transaction));
        }


        //dùng cho nạp tiền, rut tien agency
        [HttpPost]
        public async Task<IActionResult> TransactionUpdateStatus(TransactionViewModel model)
        {
            var transaction = await _ITransactionRepository.GetByIdAsync(model.Id);

            try
            {
                var wallet = _IWalletBusiness.Get(transaction.ReceiverId); // walletid receiver
                int agencyid = wallet.EntityType == EntityType.Agency ? wallet.EntityId : 0;

                if(agencyid > 0)
                {
                    if (model.Status == TransactionStatus.Completed) // duyệt thì mới +- tiền vào ví
                    {
                        

                        int code = await _ITransactionBussiness.UpdateStatus(model.Status, model.Id, HttpContext.User.Identity.Name, model.AdminNote);
                        if (code == 9)
                        {
                            TempData["MessageSuccess"] = "Update Status Success";

                            if(transaction.Type == TransactionType.WalletRecharge)
                            {
                                string _msg = string.Format("Lệnh nạp tiền {0}, với số tiền {1} đ, đã được duyệt!", transaction.Code, transaction.Amount.ToString(), model.Status.ToString());
                                await _INotificationBusiness.CreateNotificationTransactionByStatus(transaction.Id, agencyid, NotificationType.TransactionDepositeApprove, _msg, model.AdminNote);
                            }
                            else if(transaction.Type == TransactionType.WalletWithdraw)
                            {
                                string _msg = string.Format("Lệnh rút tiền {0}, với số tiền {1} đ, đã được duyệt!", transaction.Code, transaction.Amount.ToString(), model.Status.ToString());
                                await _INotificationBusiness.CreateNotificationTransactionByStatus(transaction.Id, agencyid, NotificationType.TransactionWithdrawApprove, _msg, model.AdminNote);
                            }
                            else if(transaction.Type == TransactionType.CampaignServiceCashBack)
                            {
                                string _msg = string.Format("Lệnh rút tiền {0} từ chiến dịch, với số tiền {1} đ, đã được duyệt!", transaction.Code, transaction.Amount.ToString(), model.Status.ToString());
                                await _INotificationBusiness.CreateNotificationTransactionByStatus(transaction.Id, agencyid, NotificationType.TransactionCampaignServiceCashBackApprove, _msg, model.AdminNote);
                            }
                            

                        }
                        else if (code == 10)
                        {
                            TempData["MessageError"] = "Wallet do not exist";
                        }
                        else if (code == 11)
                        {
                            TempData["MessageError"] = "Wallet balance sender or receiver less then zero or amount could be abstract";
                        }
                    }
                    else
                    {
                                                  
                        transaction.Status = model.Status;
                        transaction.DateModified = DateTime.Now;

                        transaction.AdminNote = model.AdminNote;

                        transaction.UserModified = HttpContext.User.Identity.Name;
                        await _ITransactionRepository.UpdateAsync(transaction);
                        TempData["MessageSuccess"] = "Update Status Success";

                        //####################### Gửi notification ##################################################################################
                        try
                        {
                            NotificationType _notifyType = NotificationType.TransactionDepositeCancel;
                            string _msg = string.Format("Lệnh nạp tiền {0}, với số tiền {1} đ, có trạng thái là {2}", transaction.Code, transaction.Amount.ToString(), model.Status.ToString());
                            if (model.Status == TransactionStatus.Processing)
                            {
                                if (transaction.Type == TransactionType.CampaignServiceCashBack)
                                {
                                    _notifyType = NotificationType.TransactionCampaignServiceCashBackProcessing;
                                    _msg = string.Format("Lệnh rút tiền {0} từ chiến dịch, với số tiền {1} đ, có trạng thái là {2}", transaction.Code, transaction.Amount.ToString(), model.Status.ToString());

                                }
                                else if (model.Type == TransactionType.WalletRecharge)
                                {
                                    _notifyType = NotificationType.TransactionDepositeProcessing;
                                }
                                else if (model.Type == TransactionType.WalletWithdraw)
                                {
                                    _msg = string.Format("Lệnh rút tiền {0}, với số tiền {1} đ, có trạng thái là {2}", transaction.Code, transaction.Amount.ToString(), model.Status.ToString());
                                    _notifyType = NotificationType.TransactionWithdrawProcessing;
                                }
                            }
                            await _INotificationBusiness.CreateNotificationTransactionByStatus(transaction.Id, agencyid, _notifyType, _msg, model.AdminNote);
                        }
                        catch { }
                        //################################################################################################################################

                    }
                }
                else
                {
                    TempData["MessageError"] = "Do not fit with any agency!";
                }

               

            
            }
            catch(Exception ex) {
                TempData["MessageError"] = ex.Message;
            }

            return RedirectToAction("TransactionUpdateStatus", "Transaction", new { id = model.Id, status = model.Status });
        }



        [HttpPost]
        public async Task<JsonResult> ChangeStatus(TransactionStatus status, int id)
        {
            int code = await _ITransactionBussiness.UpdateStatus(status, id, HttpContext.User.Identity.Name, "adminnote");
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
                    if(item.SenderId == 1)
                    {
                        item.SenderName = "System";
                    }
                    else
                    {
                        var wallet = _IWalletBusiness.Get(item.SenderId);
                        item.SenderName = wallet.Name;
                        item.Wallet = wallet;
                    }
                    
                }
                catch { } 
                try {
                    var wallet = _IWalletBusiness.Get(item.ReceiverId);
                    item.ReceiverName = wallet.Name;
                    item.Wallet = wallet;
                }
                catch { }
            }


            return _listTransactionViewModel;
        }
        
    }
}