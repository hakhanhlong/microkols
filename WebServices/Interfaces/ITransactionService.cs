using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
    public interface ITransactionService
    {
        Task<int> GetCount(int agencyid, TransactionType type);

        Task<TransactionViewModel> GetTransaction(TransactionType type, int RefId);

        Task<long> GetTotalAmount(int agencyid, TransactionType type);
        Task<int> CreateTransaction(EntityType entityType, int entityId, RechargeViewModel model, string username);
        Task<int> CreateTransaction(EntityType entityType, int entityId, WithDrawViewModel model, string username);
        Task<ListTransactionHistoryViewModel> GetTransactionHistory(EntityType entityType, int entityid, TransactionType type, string daterange, int page, int pagesize);
        Task<ListTransactionHistoryViewModel> GetTransactionHistory(EntityType entityType, int entityid, string daterange, int page, int pagesize);
    }
}
