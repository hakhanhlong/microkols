using Common.Extensions;
using Common.Helpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.ViewModels;

namespace Website.Services
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


        #region Create Transaction

        public async Task<int> CreateTransaction(EntityType entityType, int entityId, RechargeViewModel model, string username)
        {
            //chua viet can than can review lai -- chi khoi tao transaction - Admin duyet

            var wallet = await _walletRepository.GetWallet(entityType, entityId);
            if (wallet == null) return -1;
            var systemid = await _walletRepository.GetSystemId();
            var transactionId = await _transactionRepository.CreateTransaction(systemid, wallet.Id, model.Amount, TransactionType.WalletRecharge,
                model.GetTransactionData(), model.Note, username);
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
              data, model.Note, username);
            return transactionId;

        }


        #endregion

        #region TransactionHistory

       
        public async Task<ListTransactionHistoryViewModel> GetTransactionHistory(EntityType entityType, int entityid, string daterange, int page, int pagesize)
        {
            var date = Common.Helpers.DateRangeHelper.GetDateRange(daterange);

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







    }
}
