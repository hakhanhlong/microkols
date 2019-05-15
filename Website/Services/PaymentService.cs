using Common.Extensions;
using Core.Extensions;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.ViewModels;

namespace Website.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;

        public PaymentService(ILogger<TransactionService> logger,
            IWalletRepository walletRepository,
           ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }



    }
}
