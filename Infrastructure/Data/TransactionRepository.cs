using Common;
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

namespace Infrastructure.Data
{
    public class TransactionRepository : EfRepository<Transaction>, ITransactionRepository
    {

        public TransactionRepository(AppDbContext dbContext) : base(dbContext)
        {

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
    }
}
