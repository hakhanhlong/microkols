using BackOffice.Models;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business.Interfaces
{
    public interface ITransactionBusiness
    {
        Task<ListTransactionViewModel> GetTransactionByType(TransactionType type, int pageindex, int pagesize);
        Task<ListTransactionViewModel> GetTransactionByStatus(TransactionStatus status, int pageindex, int pagesize);
    }
}
