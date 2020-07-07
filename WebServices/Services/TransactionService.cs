using Common.Extensions;
using Common.Helpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Interfaces;
using WebServices.ViewModels;

namespace WebServices.Services
{
    public class TransactionService : BaseService, ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;

        private readonly IAsyncRepository<TransactionHistory> _transactionHistoryRepository;

        public TransactionService(ILogger<TransactionService> logger,
            IWalletRepository walletRepository, IAsyncRepository<TransactionHistory> transactionHistoryRepository,
           ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _transactionHistoryRepository = transactionHistoryRepository;
        }




        public async Task<TransactionViewModel> GetTransaction(TransactionType type, int RefId)
        {
            var transaction = await _transactionRepository.GetTransaction(type, RefId);
            if (transaction != null)
            {
                return new TransactionViewModel(transaction);
            }

            return null;
        }

        public async Task<List<TransactionViewModel>> GetTransactionForExport(TransactionType type, TransactionStatus status, DateTime startDate, DateTime endDate)
        {
            var filter = new TransactionSpecification(type, status, startDate, endDate);
            var transactions = await _transactionRepository.ListAsync(filter);
            return transactions.Select(t => new TransactionViewModel(t)).ToList();
        }

        #region Create Transaction

        public async Task<int> GetCount(int agencyid, TransactionType type)
        {
            var query = await _transactionRepository.GetQueryTransaction(EntityType.Agency, agencyid, type);
            return query.Count();
        }
        public async Task<long> GetTotalAmount(int agencyid, TransactionType type)
        {
            var query = await _transactionRepository.GetQueryTransaction(EntityType.Agency, agencyid, type);
            return query.Select(m => m.Amount).Sum();
        }



        public async Task<int> CreateTransaction(EntityType entityType, int entityId, RechargeViewModel model, string username)
        {
            //chua viet can than can review lai -- chi khoi tao transaction - Admin duyet

            var wallet = await _walletRepository.GetWallet(entityType, entityId);
            if (wallet == null) return -1;
            var systemid = await _walletRepository.GetSystemId();
            var transactionId = await _transactionRepository.CreateTransaction(systemid, wallet.Id, model.Amount, TransactionType.WalletRecharge,
                model.Note, model.GetTransactionData(), username);
            return transactionId;


        }

        public async Task<int> CreateTransaction(EntityType entityType, int entityId, WithDrawViewModel model, string username)
        {
            //chua viet can than can review lai -- chi khoi tao transaction - Admin duyet
            var wallet = await _walletRepository.GetWallet(entityType, entityId);
            if (wallet == null) return -1;

            var systemid = await _walletRepository.GetSystemId();

            var data = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                BankName = model.Bank,
                BankNumber = model.Number,
                BankAccount = model.Name,
                BankBranch = model.Branch
            });
            var transactionId = await _transactionRepository.CreateTransaction(systemid, wallet.Id, model.Amount, TransactionType.WalletWithdraw,
             model.Note, data, username);
            return transactionId;

        }


        #endregion

        #region TransactionHistory


        public async Task<ListTransactionHistoryViewModel> GetTransactionHistory(EntityType entityType, int entityid, string daterange, int page, int pagesize)
        {
            var date = Common.Helpers.DateRangeHelper.GetDateRangeByDate(daterange);

            var walletid = await _walletRepository.GetWalletId(entityType, entityid);

            var filter = new TransactionHistorySpecification(walletid, date);

            var total = await _transactionHistoryRepository.CountAsync(filter);

            var transactionHistory = await _transactionHistoryRepository.ListPagedAsync(filter, "Id_desc", page, pagesize);

            return new ListTransactionHistoryViewModel()
            {
                TransactionHistories = TransactionHistoryViewModel.GetList(transactionHistory),
                Pager = new PagerViewModel(page, pagesize, total)
            };

        }

        public async Task<ListTransactionHistoryViewModel> GetTransactionHistory(EntityType entityType, int entityid, TransactionType type,
            string daterange, int page, int pagesize)
        {
            var date = Common.Helpers.DateRangeHelper.GetDateRangeByDate(daterange);

            var walletid = await _walletRepository.GetWalletId(entityType, entityid);

            var filter = new TransactionHistorySpecification(walletid, date, type);

            var total = await _transactionHistoryRepository.CountAsync(filter);
            var transactionHistory = await _transactionHistoryRepository.ListPagedAsync(filter, "Id_desc", page, pagesize);

            return new ListTransactionHistoryViewModel()
            {
                TransactionHistories = TransactionHistoryViewModel.GetList(transactionHistory),
                Pager = new PagerViewModel(page, pagesize, total)
            };

        }
        #endregion

        #region Helper
        /*
        private async Task<int> Pay(TransactionMethod method, int customerid, string customercode, int orderid, int amount, string note, string username)
        {
            var newamount = 0;

            var paystatus = _customerRepository.Exchange(customerid, 0 - amount, username, out newamount);
            if (paystatus != 1)
            {
                return paystatus;
            }
            var transaction = new Transactions()
            {
                Amount = amount,
                BankType = TransactionBankType.KhongChon,
                Code = newamount.ToString(),
                CustomerId = customerid,
                DataType = TransactionDataType.Order,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                DataId = orderid,
                Method = method,
                Note = note,
                Status = TransactionStatus.DaDuyet,
                UserCreated = username,
                UserModified = username,
                CurrentAmount = newamount - amount
            };
            await _transactionRepository.AddAsync(transaction);
            await UpdateADHCode(transaction, customercode);
            await _transactionRepository.UpdateTmpTransaction(orderid, amount);


            return transaction.Id;
        }

        */
        #endregion


        #region Statistic

        public async Task<List<TransactionStatisticViewModel>> Statistic_CampaignServicePaid(string startDate, string endDate, TransactionStatus status)
        {
            var results = await _transactionRepository.TransactionStatisticByType(startDate, endDate, TransactionType.CampaignServiceCharge, status);
            return results.Select(t => new TransactionStatisticViewModel(t)).ToList();
        }

        public async Task<List<TransactionStatisticViewModel>> Statistic_CampaignServiceCashback(string startDate, string endDate, TransactionStatus status)
        {
            var results = await _transactionRepository.TransactionStatisticByType(startDate, endDate, TransactionType.CampaignServiceCashBack, status);
            return results.Select(t => new TransactionStatisticViewModel(t)).ToList();
        }


        public async Task<List<TransactionStatisticViewModel>> Statistic_CampaignAccountPaybackPaid(string startDate, string endDate, TransactionStatus status)
        {
            var results = await _transactionRepository.TransactionStatisticByType(startDate, endDate, TransactionType.CampaignAccountPayback, status);
            return results.Select(t => new TransactionStatisticViewModel(t)).ToList();
        }


        public async Task<CampaignDetailRevenuePieChartViewModel> Statistic_CampaignDetailRevenuePieChart(int campaignid)
        {
            var results = await _transactionRepository.TransactionStatisticCampaignRevenue(campaignid);
            if (results != null && results.Count > 0)
            {
                return new CampaignDetailRevenuePieChartViewModel(results[0]);
            }

            return null;
        }


        public async Task<CampaignDetailRevenuePieChartViewModel> Statistic_CampaignRevenuePieChart(string startDate, string endDate)
        {
            var results = await _transactionRepository.TransactionStatisticCampaignRevenue(startDate, endDate);
            if (results != null && results.Count > 0)
            {
                return new CampaignDetailRevenuePieChartViewModel(results[0]);
            }

            return null;
        }

        #endregion

        #region statistic wallet transaction
        public async Task<List<TransactionStatisticViewModel>> Statistic_Agency_WalletRecharge(int walletid, string startDate, string endDate, TransactionStatus status)
        {
            var results = await _transactionRepository.TransactionStatisticByType(walletid, startDate, endDate, TransactionType.WalletRecharge, status);
            return results.Select(t => new TransactionStatisticViewModel(t)).ToList();
        }
        public async Task<List<TransactionStatisticViewModel>> Statistic_Agency_CampaignServicePaid(int walletid, string startDate, string endDate, TransactionStatus status)
        {
            var results = await _transactionRepository.TransactionStatisticByType(walletid, startDate, endDate, TransactionType.CampaignServiceCharge, status);
            return results.Select(t => new TransactionStatisticViewModel(t)).ToList();
        }
        public async Task<List<TransactionStatisticViewModel>> Statistic_Agency_ServiceCashback(int walletid, string startDate, string endDate, TransactionStatus status)
        {
            var results = await _transactionRepository.TransactionStatisticByType(walletid, startDate, endDate, TransactionType.CampaignServiceCashBack, status);
            return results.Select(t => new TransactionStatisticViewModel(t)).ToList();
        }


        public async Task<List<TransactionStatisticViewModel>> Statistic_Influencer_CampaignAccountPayback(int walletid, string startDate, string endDate, TransactionStatus status)
        {
            var results = await _transactionRepository.TransactionStatisticByType(walletid, startDate, endDate, TransactionType.CampaignAccountPayback, status);
            return results.Select(t => new TransactionStatisticViewModel(t)).ToList();
        }

        #endregion







    }
}
