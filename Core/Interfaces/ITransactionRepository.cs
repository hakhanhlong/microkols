using Common;
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
        Task<int> CreateTransaction(int senderid, int receiverid, long amount,
           TransactionType type, string data, string note, string username);
    }
}
