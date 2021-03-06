﻿using Common;
using Common.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using System.Data.SqlClient;
using Infrastructure.Extensions;



namespace Infrastructure.Data
{
    public class TransactionRepository : EfRepository<Transaction>, ITransactionRepository
    {

        
        public TransactionRepository(AppDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<Transaction> GetTransaction(TransactionType type, int RefId)
        {
            var query = await _dbContext.Transaction.FirstOrDefaultAsync(m => m.Type == type && m.RefId == RefId);
            return query;

        }

        public async Task<IQueryable<Transaction>> GetQueryTransaction(EntityType entityType, int entityId, TransactionType type)
        {
            var wallet = await _dbContext.Wallet.Where(m => m.EntityId == entityId && m.EntityType == entityType).FirstOrDefaultAsync();
            if (wallet != null)
            {


                var query = _dbContext.Transaction.Where(m => m.Type == type && m.Status == TransactionStatus.Completed);
                if (type == TransactionType.WalletWithdraw)
                {
                    query = query.Where(m => m.SenderId == wallet.Id);

                }
                else
                {
                    query = query.Where(m => m.ReceiverId == wallet.Id);
                }

                return query;

            }
            return new List<Transaction>().AsQueryable();


        }

        public async Task<bool> IsExistPaymentServiceCashBack(int agencyId, int campaignid)
        {
            var translate = await _dbContext.Transaction.FirstOrDefaultAsync(m=>m.Type== TransactionType.CampaignServiceCashBack &&
            m.RefId== campaignid && m.SenderId== agencyId && m.Status== TransactionStatus.Created);

            return translate != null;
        }
        public async Task<int> CreateTransaction(int senderid, int receiverid, long amount,
            TransactionType type, string note, string data, string username, int refId = 0, string refData = "")
        {
            var code = $"{(int)type}{Common.Helpers.StringHelper.GetHexTime()}";
            var transaction = new Transaction()
            {
                Amount = amount,
                Code = code,
                RefData = refData,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Note = note,
                ReceiverId = receiverid,
                SenderId = senderid,
                Status = TransactionStatus.Created,
                Type = type,
                UserCreated = username,
                UserModified = username,
                RefId = refId,
                AdminNote = string.Empty,
                Data = data
            };
            await _dbContext.Transaction.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction.Id;

        }

        public async Task<int> CreateTransaction(int senderid, int receiverid, long amount, long amountOriginal,
           TransactionType type, string note, string data, string username, int refId = 0, string refData = "")
        {
            var code = $"{(int)type}{Common.Helpers.StringHelper.GetHexTime()}";
            var transaction = new Transaction()
            {
                Amount = amount,
                AmountOriginal = amountOriginal,
                Code = code,
                RefData = refData,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Note = note,
                ReceiverId = receiverid,
                SenderId = senderid,
                Status = TransactionStatus.Created,
                Type = type,
                UserCreated = username,
                UserModified = username,
                RefId = refId,
                AdminNote = string.Empty,
                Data = data
            };
            await _dbContext.Transaction.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction.Id;

        }




        public async Task<bool> UpdateTransactionStatus(int id, TransactionStatus status, string note, string username)
        {
            var transaction = await _dbContext.Transaction.FindAsync(id);
            if (transaction != null)
            {
                transaction.Status = status;
                transaction.DateModified = DateTime.Now;
                transaction.UserModified = username;
                transaction.AdminNote = note;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task UpdateTransactionHistory(int transactionid, int walletid, long amount, long balance, string note)
        {
            var transactionHistory = new TransactionHistory()
            {
                Amount = amount,
                Balance = balance,
                DateCreated = DateTime.Now,
                Note = note,
                TransactionId = transactionid,
                WalletId = walletid
            };
            await _dbContext.TransactionHistory.AddAsync(transactionHistory);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<long> GetTotalAmount(TransactionType transactionType, int refid)
        {

            return await _dbContext.Transaction.Where(m => m.Type == transactionType && m.RefId == refid && m.Status == TransactionStatus.Completed).Select(m => m.Amount).DefaultIfEmpty(0).SumAsync();
        }



        public int CountAll()
        {
            return _dbContext.Transaction.Count();
        }


        #region Statistic

        public async Task<List<TransactionStatistic>> TransactionStatisticByType(string startDate, string endDate, TransactionType type, TransactionStatus status)
        {
            List<TransactionStatistic> result = new List<TransactionStatistic>();        
            result = await _dbContext.LoadStoredProc("sp_transaction_statistic_campaign_by_type")
                .WithSqlParam("StartDate", startDate)
                .WithSqlParam("EndDate", endDate)
                .WithSqlParam("Type", type)
                .WithSqlParam("Status", status)
                .ExecuteStoredProc<TransactionStatistic>();
            return result;
        }

        public async Task<List<TransactionStatistic>> TransactionStatisticByType(int walletid, string startDate, string endDate, TransactionType type, TransactionStatus status)
        {
            List<TransactionStatistic> result = new List<TransactionStatistic>();
            result = await _dbContext.LoadStoredProc("sp_transaction_statistic_wallet_by_id_filter_by_type")
                .WithSqlParam("WalletId", walletid)
                .WithSqlParam("StartDate", startDate)
                .WithSqlParam("EndDate", endDate)
                .WithSqlParam("Type", type)
                .WithSqlParam("Status", status)
                .ExecuteStoredProc<TransactionStatistic>();
            return result;
        }

        public async Task<List<TransactionCampaignRevenue>> TransactionStatisticCampaignRevenue(int campaignid)
        {
            List<TransactionCampaignRevenue> result = new List<TransactionCampaignRevenue>();

            result = await _dbContext.LoadStoredProc("sp_transaction_statistic_revenue_campaign_detail")
                .WithSqlParam("CampaignId", campaignid)                
                .ExecuteStoredProc<TransactionCampaignRevenue>();

            return result;
        }

        public async Task<List<TransactionCampaignRevenue>> TransactionStatisticCampaignRevenue(string startDate, string endDate)
        {
            List<TransactionCampaignRevenue> result = new List<TransactionCampaignRevenue>();

            result = await _dbContext.LoadStoredProc("sp_transaction_statistic_revenue_campaign")
                .WithSqlParam("StartDate", startDate)
                .WithSqlParam("EndDate", endDate)
                .ExecuteStoredProc<TransactionCampaignRevenue>();

            return result;
        }


        #endregion
    }
}
