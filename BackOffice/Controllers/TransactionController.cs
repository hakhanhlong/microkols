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
using BackOffice.Extensions;
using Common.Extensions;

namespace BackOffice.Controllers
{

    [Authorize]
    public class TransactionController : Controller
    {
        ITransactionBusiness _ITransactionBussiness;
        ITransactionService _ITransactionService;

        ITransactionRepository _ITransactionRepository;
        ICampaignService _ICampaignService;

        IWalletBusiness _IWalletBusiness;
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";        
        IPayoutExportRepository _IPayoutExportRepository;
        private readonly INotificationBusiness _INotificationBusiness;
        private readonly ITransactionHistoryBusiness _ITransactionHistoryBusiness;
        private readonly IPaymentService _paymentService;

        private readonly IPayoutExportService _IPayoutExportService;


        public TransactionController(ITransactionBusiness __ITransactionBusiness, IWalletBusiness __IWalletBusiness, 
            IPayoutExportRepository __IPayoutExportRepository, ITransactionRepository __ITransactionRepository, INotificationBusiness __INotificationBusiness, 
            ITransactionHistoryBusiness __ITransactionHistoryBusiness, ITransactionService __ITransactionService, ICampaignService __ICampaignService,
            IPaymentService _IPaymentService, IPayoutExportService __IPayoutExportService)
        {
            _ITransactionBussiness = __ITransactionBusiness;
            _IWalletBusiness = __IWalletBusiness;
            _IPayoutExportRepository = __IPayoutExportRepository;
            _ITransactionRepository = __ITransactionRepository;
            _INotificationBusiness = __INotificationBusiness;
            _ITransactionHistoryBusiness = __ITransactionHistoryBusiness;
            _ITransactionService = __ITransactionService;

            _ICampaignService = __ICampaignService;
            _paymentService = _IPaymentService;
            _IPayoutExportService = __IPayoutExportService;
        }

        public IActionResult Index()
        {
            return View();
        }


        private void BindingTransactionOptions()
        {
            ViewBag.TransactionStatus = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Tất cả", Value = "-1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Khởi tạo", Value = "0"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Hủy bỏ", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Đang xử lý", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Hoàn thành", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Lỗi", Value = "4"},
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
            var _listTransaction = await FillTransactions(TransactionType.CampaignAccountPayback, status, pageindex);
            return View(_listTransaction);
        }

        public async Task<IActionResult> History(int transactionid, int walletid)
        {
            var transactionhistories = await _ITransactionHistoryBusiness.GetByTransactionID(transactionid);

            return View(transactionhistories);
        }


        #region AccountPayback

        public async Task<IActionResult> PayoutExport(AccountType type = AccountType.Regular, int pageindex = 1, int pagesize = 25)
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

            try {
                var lastDateTime = DateTime.Now.AddMonths(-1);
                DateTime startDate = new DateTime(lastDateTime.Year, lastDateTime.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                bool isexist = _IPayoutExportRepository.IsExist(startDate, endDate, type);
                if (!isexist)
                {
                    _IPayoutExportRepository.Add(new PayoutExport()
                    {
                        AccountType = type,
                        CreatedDate = DateTime.Now,
                        CreatedUser = HttpContext.User.Identity.Name,
                        IsExport = false,
                        IsUpdateWallet = false,
                        StartDateExport = startDate,
                        EndDateExport = endDate
                    });
                }
            }
            catch { }
            
            var listing = await _IPayoutExportService.ListPayoutExport(_accounttype, pageindex, pagesize);                                  
            return View(listing);
        }



        public async Task<IActionResult> AccountPayback(AccountType type = AccountType.Regular,int pageindex = 1, int pagesize = 25, int payoutid = 0)
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

            

            var PayoutExport = await _IPayoutExportService.GetPayoutExport(payoutid);

            var _listTransaction = await _ITransactionBussiness.GetTotalPayoutTransactions(TransactionType.CampaignAccountPayback, TransactionStatus.Completed, _accounttype.ToList(), PayoutExport.StartDateExport.Date, PayoutExport.EndDateExport.Date, pageindex, pagesize);

            ViewBag.PayoutExport = PayoutExport;

            return View(_listTransaction);
        }

        public async Task<IActionResult> AccountPaybackDetail(int accountid)
        {
            //var _listTransaction = await FillTransactions(TransactionType.CampaignAccountPayback, status, pageindex);
          
            var lastDateTime = DateTime.Now.AddMonths(-1);
            DateTime startDate = new DateTime(lastDateTime.Year, lastDateTime.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            
            var _listTransaction = await _ITransactionBussiness.GetPayoutTransactionByAccountId(TransactionType.CampaignAccountPayback, TransactionStatus.Completed, accountid);
            

            return View(_listTransaction);
        }
        #endregion




        public async Task<IActionResult> AccountSubtractWallet(AccountType type = AccountType.All, int payoutid = 0)
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

            
            int count_fail = 0;

            var PayoutExport = await _IPayoutExportService.GetPayoutExport(payoutid);
            var _listTransaction = await _ITransactionBussiness.GetTotalPayoutTransactions(TransactionType.CampaignAccountPayback, TransactionStatus.Completed, _accounttype.ToList(), PayoutExport.StartDateExport.Date, PayoutExport.EndDateExport.Date);



            if (_listTransaction.Transactions.Count > 0)
            {

                foreach(var item in _listTransaction.Transactions)
                {

                    if (item.Wallet.Balance == 0 || item.Wallet.Balance < item.TotalCashOut)
                        continue;

                    int senderid = item.Wallet.Id;
                    int receiverid = 1; //walletid of system
                    long money_number = item.TotalCashOut;
                    int campaignid = 0;
                    int accountid = item.Account.Id;

                    string txt_note = string.Format("Hệ thống thực hiện trừ tiền {0} đ trên ví walletid = {1},{2}, " +
                        "vì hệ thống đã chuyển {3} đ đến tài khoản ngân hàng của thành viên {4} thuộc walletid={5}. Và cộng {6} đ vào ví walletid={7}",
                        money_number, senderid, item.Account.Name, money_number, item.Account.Name, senderid, money_number, receiverid);

                    int transactionid = await _ITransactionRepository.CreateTransaction(senderid, receiverid, money_number, TransactionType.ExcecutedPaymentToAccountBanking, txt_note, string.Format("Campaign ID = {0}", campaignid), HttpContext.User.Identity.Name, campaignid);

                    if (transactionid > 0) // nếu tạo transaction thành công
                    {
                        int retResult = await _ITransactionBussiness.CalculateBalance(transactionid, money_number, senderid, receiverid, "[Chuyển tiền đến tài khoản ngân hàng của thành viên][ExcecutedPaymentToAccountBanking]", HttpContext.User.Identity.Name);
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
                                    //int retValue = _ITransactionBussiness.UpdateCashOut(item.Id);
                                    await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Completed, "Success", HttpContext.User.Identity.Name);//
                                    //await _ITransactionRepository.UpdateTransactionStatus(item.Id, TransactionStatus.Completed, "Success", HttpContext.User.Identity.Name);// delete transaction if case error
                                    NotificationType notificationType = NotificationType.ExcecutedPaymentToAccountBanking;
                                    string msg = string.Format("Hệ thống đã chuyển tiền {0} đ tới tài khoản ngân hàng của bạn và tự động trừ tiền trong ví tương ứng với số tiền {1}, " +
                                        "từ chiến dịch bạn đã hoàn thành.", money_number, money_number);
                                    await _INotificationBusiness.CreateNotificationExcecutedPaymentToAccountBanking(campaignid, accountid, notificationType, msg, "");
                                    try {
                                        var payoutexportEntity = _IPayoutExportRepository.GetById(payoutid);
                                        payoutexportEntity.IsUpdateWallet = true;
                                        payoutexportEntity.PayoutPayDate = DateTime.Now;
                                        _IPayoutExportRepository.Update(payoutexportEntity);
                                    }
                                    catch
                                    {}
                                    

                                    break;
                                case 10:
                                    await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Error, "Wallet do not exist", HttpContext.User.Identity.Name);// delete transaction if case error
                                    //await _ITransactionRepository.UpdateTransactionStatus(item.Id, TransactionStatus.Completed, "Wallet do not exist", HttpContext.User.Identity.Name);// delete transaction if case error
                                    count_fail++;
                                    break;
                                case 11:
                                    await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Error, "Wallet balance sender or receiver less then zero or amount could be abstract", HttpContext.User.Identity.Name);// delete transaction if case error
                                    //await _ITransactionRepository.UpdateTransactionStatus(item.Id, TransactionStatus.Completed, "Wallet balance sender or receiver less then zero or amount could be abstract", HttpContext.User.Identity.Name);// delete transaction if case error
                                    count_fail++;
                                    break;
                                case 12:
                                    await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Error, "Wallet balance sender do not enought balance", HttpContext.User.Identity.Name);// delete transaction if case error
                                    //await _ITransactionRepository.UpdateTransactionStatus(item.Id, TransactionStatus.Completed, "Wallet balance sender do not enought balance", HttpContext.User.Identity.Name);// delete transaction if case error
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

            return RedirectToAction("AccountPayback", "Transaction", new { type = type , payoutid  = payoutid });

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

        public async Task<IActionResult> ExportAccountPayback(AccountType type = AccountType.All, int payoutid = 0)
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

            //var _listTransaction = await _ITransactionBussiness.GetPayoutTransactions(TransactionType.CampaignAccountPayback, TransactionStatus.Completed,  _accounttype.ToList());
            var PayoutExport = await _IPayoutExportService.GetPayoutExport(payoutid);
            var _listTransaction = await _ITransactionBussiness.GetTotalPayoutTransactions(TransactionType.CampaignAccountPayback, TransactionStatus.Completed, _accounttype.ToList(), PayoutExport.StartDateExport.Date, PayoutExport.EndDateExport.Date, 1, 500);
            if (_listTransaction != null)
            {

                var package = new ExcelPackage();
                package.Workbook.Properties.Title = "AccountPayback";
                package.Workbook.Properties.Author = "MicroKols.";
                package.Workbook.Properties.Subject = "Account Payback";
                package.Workbook.Properties.Keywords = "AccountPayback";


                var worksheet = package.Workbook.Worksheets.Add("Danh sách tài khoản tất toán");

                int count_row_header = 1;
                int number_stt = 1;
                


                //First add the headers
                worksheet.Cells[count_row_header, 1].Value = "STT";
                worksheet.Cells[count_row_header, 1].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 2].Value = "Tên tài khoản ";
                worksheet.Cells[count_row_header, 2].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 3].Value = "Số tài khoản";
                worksheet.Cells[count_row_header, 3].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 4].Value = "Ngân hàng";
                worksheet.Cells[count_row_header, 4].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 5].Value = "Chi nhánh";
                worksheet.Cells[count_row_header, 5].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 6].Value = "Tiền tất toán";
                worksheet.Cells[count_row_header, 6].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 7].Value = "Ghi chú";
                worksheet.Cells[count_row_header, 7].Style.Font.Bold = true;


                foreach (var transaction in _listTransaction.Transactions)
                {

                    count_row_header++;

                    

                    worksheet.Cells[count_row_header, 1].Value = number_stt;
                    worksheet.Cells[count_row_header, 2].Value = transaction.Account.BankAccountName;
                    worksheet.Cells[count_row_header, 3].Value = transaction.Account.BankAccountNumber;
                    worksheet.Cells[count_row_header, 4].Value = transaction.Account.BankAccountBank;
                    worksheet.Cells[count_row_header, 5].Value = transaction.Account.BankAccountBranch;
                    worksheet.Cells[count_row_header, 6].Value = transaction.TotalCashOut.ToPriceText();

                    if(transaction.Wallet.Balance < transaction.TotalCashOut)
                    {
                        worksheet.Cells[count_row_header, 7].Value = "Ví không đủ để thực hiện việc tất toán";
                    }
                    else
                    {
                        worksheet.Cells[count_row_header, 7].Value = "";
                    }
                    


                    number_stt++;
                }


                byte[] reportBytes = new byte[] { };
                try {
                    reportBytes = package.GetAsByteArray();
                    if(reportBytes.Length > 0)
                    {
                        var payoutexportEntity = _IPayoutExportRepository.GetById(payoutid);
                        payoutexportEntity.IsExport = true;
                        payoutexportEntity.PayoutExportFileDate = DateTime.Now;
                        _IPayoutExportRepository.Update(payoutexportEntity);

                    }                    
                }
                catch { }
                
                return File(reportBytes, XlsxContentType, string.Format("{0}{1}{2}_{3}{4}{5}_{6}.xlsx", PayoutExport.StartDateExport.Day, PayoutExport.StartDateExport.Month, 
                    PayoutExport.StartDateExport.Year, PayoutExport.EndDateExport.Day,
                    PayoutExport.EndDateExport.Month, PayoutExport.EndDateExport.Year, type.ToString()));
               

            }

            return RedirectToAction("AccountPayback", "Transaction", new { type = type , payoutid = payoutid });
        }


        public async Task<IActionResult> ExportTransactionCampaignServiceCharge(DateTime start, DateTime end)
        {

            var _listTransaction = await _ITransactionService.GetTransactionForExport(TransactionType.CampaignServiceCharge, TransactionStatus.Completed, start, end);

            if (_listTransaction != null)
            {
                var package = new ExcelPackage();
                package.Workbook.Properties.Title = "Phí dịch vụ của chiến dịch";
                package.Workbook.Properties.Author = "MicroKols.";
                package.Workbook.Properties.Subject = "Campaign Service Charge";
                package.Workbook.Properties.Keywords = "Campaign Service Charge";


                var worksheet = package.Workbook.Worksheets.Add("Phí dịch vụ các chiến dịch chiến dịch");

                int count_row_header = 1;
                int number_stt = 1;
                //First add the headers
                worksheet.Cells[count_row_header, 1].Value = "STT";
                worksheet.Cells[count_row_header, 1].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 2].Value = "Tên chiến dịch";
                worksheet.Cells[count_row_header, 2].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 3].Value = "Loại chiến dịch";
                worksheet.Cells[count_row_header, 3].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 4].Value = "Doanh nghiệp tạo";
                worksheet.Cells[count_row_header, 4].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 5].Value = "Chi phí trả Influencer(vnđ)";
                worksheet.Cells[count_row_header, 5].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 6].Value = "Phí dịch vụ (%)";
                worksheet.Cells[count_row_header, 6].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 7].Value = "Thành tiền phí dịch vụ (%)";
                worksheet.Cells[count_row_header, 7].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 8].Value = "Tổng chi phí (vnđ)";
                worksheet.Cells[count_row_header, 8].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 9].Value = "Thời gian bắt đầu";
                worksheet.Cells[count_row_header, 9].Style.Font.Bold = true;
                worksheet.Cells[count_row_header, 10].Value = "Thời gian kết thúc";
                worksheet.Cells[count_row_header, 10].Style.Font.Bold = true;

                try
                {
                    foreach (var transaction in _listTransaction)
                    {

                        count_row_header++;

                        var campaign = await _ICampaignService.GetCampaignById(transaction.RefId.Value);

                        long tien_phidichvu = (transaction.AmountOriginal * campaign.ServiceChargePercent) / 100;

                        worksheet.Cells[count_row_header, 1].Value = number_stt; //stt
                        worksheet.Cells[count_row_header, 2].Value = campaign.Title; //tên chiến dịch
                        worksheet.Cells[count_row_header, 3].Value = campaign.Type.ToText(); //loại chiến dịch
                        worksheet.Cells[count_row_header, 4].Value = campaign.Agency.Name; //doanh nghiệp tạo
                        worksheet.Cells[count_row_header, 5].Value = transaction.AmountOriginal; //chi phí trả influencer
                        worksheet.Cells[count_row_header, 6].Value = campaign.ServiceChargePercent;//phí dịch vụ
                        worksheet.Cells[count_row_header, 7].Value = tien_phidichvu;//thành tiền phí dịch vụ
                        worksheet.Cells[count_row_header, 8].Value = (transaction.AmountOriginal + tien_phidichvu);//tổng chi phí
                        worksheet.Cells[count_row_header, 9].Value = campaign.ExecutionStart.Value.ToShortDateString();//thời gian bắt đầu
                        worksheet.Cells[count_row_header, 10].Value = campaign.ExecutionEnd.Value.ToShortDateString();//thời gian kết thúc


                        number_stt++;
                    }

              
                    byte[] reportBytes = new byte[] { };
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, string.Format("chiphichiendich_{0}{1}{2}_{3}{4}{5}.xlsx", start.Day, start.Month, start.Year, end.Day, end.Month, end.Year));
                }
                catch(Exception ex) {
                    TempData["MessageError"] = ex.Message;
                }                
            }

            TempData["MessageInfo"] = "Không có dữ liệu chi phí chiến dịch để xuất file.";
            return RedirectToAction("Index", "Home");



        }






        //dùng cho nạp tiền agency
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

                //################## transaction type = các dạng sau mới đi tiếp và xử lý ####################################################
                //############################################################################################################################
                if(transaction.Type == TransactionType.WalletRecharge || transaction.Type == TransactionType.WalletWithdraw || transaction.Type == TransactionType.CampaignServiceCashBack)
                {
                    var wallet = _IWalletBusiness.Get(transaction.ReceiverId); // walletid receiver
                    int agencyid = wallet.EntityType == EntityType.Agency ? wallet.EntityId : 0;

                    if(transaction.Type == TransactionType.WalletWithdraw) //withdraw phía doanh nghiệp
                    {
                        wallet = _IWalletBusiness.Get(transaction.SenderId);
                        agencyid = wallet.EntityType == EntityType.Agency ? wallet.EntityId : 0;
                    }



                    if (agencyid > 0)
                    {
                        if (model.Status == TransactionStatus.Completed) // duyệt thì mới +- tiền vào ví
                        {


                            int code = await _ITransactionBussiness.UpdateStatus(model.Status, model.Id, HttpContext.User.Identity.Name, model.AdminNote);
                            if (code == 9)
                            {
                                TempData["MessageSuccess"] = "Update Status Success";

                                if (transaction.Type == TransactionType.WalletRecharge)
                                {
                                    string _msg = string.Format("Lệnh nạp tiền {0}, với số tiền {1} đ, đã được duyệt!", transaction.Code, transaction.Amount.ToString(), model.Status.ToShowName().ToString());
                                    await _INotificationBusiness.CreateNotificationTransactionByStatus(transaction.Id, agencyid, NotificationType.TransactionDepositeApprove, _msg, model.AdminNote);
                                }
                                else if (transaction.Type == TransactionType.WalletWithdraw)
                                {
                                    string _msg = string.Format("Lệnh rút tiền {0}, từ ví với số tiền {1} đ, đã được duyệt!", transaction.Code, transaction.Amount.ToString(), model.Status.ToShowName().ToString());
                                    await _INotificationBusiness.CreateNotificationTransactionByStatus(transaction.Id, agencyid, NotificationType.TransactionWithdrawApprove, _msg, model.AdminNote);
                                }
                                else if (transaction.Type == TransactionType.CampaignServiceCashBack)
                                {
                                    string _msg = string.Format("Lệnh rút tiền {0} từ chiến dịch, với số tiền {1} đ, đã được duyệt!", transaction.Code, transaction.Amount.ToString(), model.Status.ToShowName().ToString());
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
                                string _msg = string.Format("Lệnh nạp tiền {0}, với số tiền {1} đ, có trạng thái là {2}", transaction.Code, transaction.Amount.ToString(), model.Status.ToShowName().ToString());
                                if (model.Status == TransactionStatus.Processing)
                                {
                                    if (transaction.Type == TransactionType.CampaignServiceCashBack)
                                    {
                                        _notifyType = NotificationType.TransactionCampaignServiceCashBackProcessing;
                                        _msg = string.Format("Lệnh rút tiền {0} từ chiến dịch, với số tiền {1} đ, có trạng thái là {2}", transaction.Code, transaction.Amount.ToString(), model.Status.ToShowName().ToString());

                                    }
                                    else if (transaction.Type == TransactionType.WalletRecharge)
                                    {
                                        _notifyType = NotificationType.TransactionDepositeProcessing;
                                    }
                                    else if (transaction.Type == TransactionType.WalletWithdraw)
                                    {
                                        _msg = string.Format("Lệnh rút tiền {0}, từ ví với số tiền {1} đ, có trạng thái là {2}", transaction.Code, transaction.Amount.ToString(), model.Status.ToShowName().ToString());
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
                else if(transaction.Type == TransactionType.CampaignAccountPayback)// influencer nhận thanh toán về ví
                {
                    var result = await _paymentService.Pay(transaction.Id, transaction.SenderId, transaction.ReceiverId, transaction.Amount, 
                        TransactionType.CampaignAccountPayback, model.AdminNote, HttpContext.User.Identity.Name);

                    if(result.ErrorCode == WebServices.ViewModels.PaymentResultErrorCode.KhongLoi)
                    {                        
                        int campaignid = 0;
                        if (transaction.RefId.HasValue)
                        {
                            campaignid = transaction.RefId.Value;
                        }
                        var campaign = await _ICampaignService.GetCampaignById(campaignid);
                        if(campaign != null)
                        {
                            string _msg = string.Format("Bạn đã được duyệt cộng {0} vào ví. Từ chiến dịch \"{1}\"", transaction.Amount, campaign.Title);
                            await _INotificationBusiness.CreateNotification(EntityType.Account, transaction.ReceiverId, campaignid, NotificationType.SystemSendNotifycation, _msg, string.Empty);
                        }                        
                    }
                }
                else
                {
                    TempData["MessageError"] = "Transaction không thuộc trường hợp để duyệt!";
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
                    item.WalletReceiver = wallet;
                }
                catch { }
            }


            return _listTransactionViewModel;
        }
        
    }
}