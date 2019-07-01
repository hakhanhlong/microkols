using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business
{
    public class TransactionBusiness: ITransactionBusiness
    {

        ITransactionRepository _ITransactionRepository;

        public TransactionBusiness(ITransactionRepository __ITransactionRepository) {
            _ITransactionRepository = __ITransactionRepository;
        }

        public async Task<ListTransactionViewModel> GetTransactionByType(TransactionType type, int pageindex, int pagesize)
        {
            var filter = new TransactionSpecification(type);

            var transactions = await _ITransactionRepository.ListPagedAsync(filter, "DateModified_desc", pageindex, pagesize);

            var total = await _ITransactionRepository.CountAsync(filter);

            return new ListTransactionViewModel()
            {
                Transactions = transactions.Select(t => new TransactionViewModel(t)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };

        }


        public async Task<ListTransactionViewModel> GetTransactionByStatus(TransactionStatus status, int pageindex, int pagesize)
        {
            var filter = new TransactionSpecification(status);
            var transactions = await _ITransactionRepository.ListPagedAsync(filter, "DateModified_desc", pageindex, pagesize);
            var total = await _ITransactionRepository.CountAsync(filter);

            return new ListTransactionViewModel()
            {
                Transactions = transactions.Select(t => new TransactionViewModel(t)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };

        }

    }
}
