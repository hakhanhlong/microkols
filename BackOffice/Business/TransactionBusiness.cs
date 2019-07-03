using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business
{
    public class TransactionBusiness: ITransactionBusiness
    {

        ITransactionRepository _ITransactionRepository;
        private readonly ILogger<TransactionBusiness> _logger;

        public TransactionBusiness(ITransactionRepository __ITransactionRepository, ILoggerFactory _loggerFactory) {
            _ITransactionRepository = __ITransactionRepository;
            _logger = _loggerFactory.CreateLogger<TransactionBusiness>();
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

        public async Task<ListTransactionViewModel> GetTransactions(TransactionType type, TransactionStatus status, int pageindex, int pagesize)
        {
            var filter = new TransactionSpecification(type, status);
            var transactions = await _ITransactionRepository.ListPagedAsync(filter, "DateModified_desc", pageindex, pagesize);
            var total = await _ITransactionRepository.CountAsync(filter);

            return new ListTransactionViewModel()
            {
                Transactions = transactions.Select(t => new TransactionViewModel(t)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }

        public ListTransactionViewModel GetTransactions(int pageindex, int pagesize)
        {
            var transactions = _ITransactionRepository.ListPaging("DateModified_desc", pageindex, pagesize);

            var total = _ITransactionRepository.CountAll();

            return new ListTransactionViewModel()
            {
                Transactions = transactions.Select(t => new TransactionViewModel(t)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }

    }
}
