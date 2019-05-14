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

        public TransactionService(ILogger<TransactionService> logger,
            IWalletRepository walletRepository,
           ITransactionRepository transactionRepository) 
        {
            _logger = logger;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }


        #region Create Transaction

        public async Task<int> CreateTransaction(EntityType entityType, int entityId, RechargeViewModel model, string username)
        {

            var wallet = await _walletRepository.GetWallet(entityType, entityId);
            if (wallet == null) return -1;
            var code = $"{StringHelper.GetHexTime()}_{entityType}{entityId}";

            var transactionId = await _transactionRepository.CreateTransaction(0, wallet.Id, code, model.Amount, TransactionType.WalletRecharge,
                model.GetTransactionData(),model.Note, username  );
            return transactionId;


        }

        public async Task<int> CreateTransaction(EntityType entityType, int entityId, WithDrawViewModel model, string username)
        {

            var wallet = await _walletRepository.GetWallet(entityType, entityId);
            if (wallet == null) return -1;
            var code = $"{StringHelper.GetHexTime()}_{entityType}{entityId}";



            var transactionId = await _transactionRepository.CreateTransaction(0, wallet.Id, code, model.Amount, TransactionType.WalletWithdraw,
              string.Empty, model.Note, username);
            return transactionId;

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
