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
             TransactionType type, string note, string data, string username, int refId = 0, string refData = "");

        Task<bool> UpdateTransactionStatus(int id, TransactionStatus status, string note, string username);
        Task UpdateTransactionHistory(int transactionid, int walletid, long amount, long balance, string note);

        int CountAll();

        Task<long> GetTotalAmount(TransactionType transactionType, int refid);
    }
}
