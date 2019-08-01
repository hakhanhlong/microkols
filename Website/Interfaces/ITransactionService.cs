using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.ViewModels;

namespace Website.Interfaces
{
    public interface ITransactionService
    {
        Task<int> CreateTransaction(EntityType entityType, int entityId, RechargeViewModel model, string username);
        Task<int> CreateTransaction(EntityType entityType, int entityId, WithDrawViewModel model, string username);

        Task<ListTransactionHistoryViewModel> GetTransactionHistory(EntityType entityType, int entityid, string daterange, int page, int pagesize);
    }
}
