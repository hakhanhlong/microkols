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

        public async Task<int> CreateTransaction(int senderid, int receiverid, string code, int amount, 
            TransactionType type, string data, string note, string username)
        {
            var transaction = new Transaction()
            {
                Amount = amount,
                Code = code,
                Data = data,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Note = note,
                ReceiverId = receiverid,
                SenderId = senderid,
                Status = TransactionStatus.Created,
                Type = type,
                UserCreated = username,
                UserModified = username,
            };
            await _dbContext.Transaction.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction.Id;

        }
    


    }
}
