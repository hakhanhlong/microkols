﻿using Common;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>, IAsyncRepository<Transaction>
    {
        Task<Transaction> GetTransaction(TransactionType type, int RefId);

        Task<IQueryable<Transaction>> GetQueryTransaction(EntityType entityType, int entityId, TransactionType type);

        Task<int> CreateTransaction(int senderid, int receiverid, long amount,
             TransactionType type, string note, string data, string username, int refId = 0, string refData = "");

        Task<int> CreateTransaction(int senderid, int receiverid, long amount, long amountOriginal,
           TransactionType type, string note, string data, string username, int refId = 0, string refData = "");
        


            Task<bool> UpdateTransactionStatus(int id, TransactionStatus status, string note, string username);
        Task UpdateTransactionHistory(int transactionid, int walletid, long amount, long balance, string note);

        int CountAll();

        Task<long> GetTotalAmount(TransactionType transactionType, int refid);        

        Task<bool> IsExistPaymentServiceCashBack(int agencyId, int campaignid);



        #region Statistic
        Task<List<TransactionStatistic>> TransactionStatisticByType(string startDate, string endDate, TransactionType type, TransactionStatus status);

        Task<List<TransactionStatistic>> TransactionStatisticByType(int walletid, string startDate, string endDate, TransactionType type, TransactionStatus status);


        Task<List<TransactionCampaignRevenue>> TransactionStatisticCampaignRevenue(int campaignid);
        Task<List<TransactionCampaignRevenue>> TransactionStatisticCampaignRevenue(string startDate, string endDate);

        #endregion
    }
}
