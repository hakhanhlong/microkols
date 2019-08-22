using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business
{
    public class TransactionHistoryBusiness: ITransactionHistoryBusiness
    {
        ITransactionRepository _ITransactionRepository;
        ITransactionHistoryRepository _ITransactionHistoryRepository;
        IWalletRepository _IWalletRepository;
        IAccountRepository _IAccountRepository;

        private readonly ILogger<TransactionHistoryBusiness> _logger;

        public TransactionHistoryBusiness(ITransactionRepository __ITransactionRepository,
            ILoggerFactory _loggerFactory, IWalletRepository __IWalletRepository, IAccountRepository __IAccountRepository, 
            ITransactionHistoryRepository __ITransactionHistoryRepository)
        {
            _ITransactionRepository = __ITransactionRepository;
            _logger = _loggerFactory.CreateLogger<TransactionHistoryBusiness>();
            _IWalletRepository = __IWalletRepository;
            _IAccountRepository = __IAccountRepository;
            _ITransactionHistoryRepository = __ITransactionHistoryRepository;
        }

        public async Task<ListTransactionHistoryViewModel> GetByWalletID(int walletid, int pageindex)
        {
            var filter = new TransactionHistorySpecification(walletid);
            var list_history = await _ITransactionHistoryRepository.ListPagedAsync(filter, "DateCreated_desc", pageindex);
            int pagesize = 25;
            var total = await _ITransactionHistoryRepository.CountAsync(filter);

            return new ListTransactionHistoryViewModel()
            {
                TransactionHistories = list_history.Select(t => new TransactionHistoryViewModel(t)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };

        }
    }
}
