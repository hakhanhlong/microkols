using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class TransactionHistoryRepository: EfRepository<TransactionHistory>, ITransactionHistoryRepository
    {
        public TransactionHistoryRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
