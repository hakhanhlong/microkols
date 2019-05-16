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
        private readonly ICampaignRepository _campaignRepository;
        private readonly IWalletRepository _walletRepository;

        public PaymentService(ILogger<TransactionService> logger,
            IWalletRepository walletRepository, ICampaignRepository campaignRepository,
           ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _walletRepository = walletRepository;
            _campaignRepository = campaignRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<int> CreatePayment(int agencyId, CreateCampaignPaymentViewModel model, string username)
        {
            var payment = await _campaignRepository.GetCampaignPaymentByAgency(agencyId, model.CampaignId);
            if (payment == null)
            {
                return -1;
            }
            long amount = 0;
            var senderId = 0;
            var receiverId = 0;
            var transactionType = Core.Entities.TransactionType.Undefined;

            if (payment.IsValidServiceCharge)
            {
                //service charge --> tru tien cho user ; + tien cho he thong
                receiverId = await _walletRepository.GetSystemId();
                senderId = await _walletRepository.GetWalletId(Core.Entities.EntityType.Agency, agencyId);
                amount = payment.ServiceChargeValue;
                transactionType = Core.Entities.TransactionType.CampaignServiceCharge;
                // check lan` 1 
            }

            var transactionid = await _transactionRepository.CreateTransaction(agencyId, receiverId, amount, transactionType, "", model.Note, username);
            if (transactionid > 0)
            {
                //tru tien
                await _walletRepository.Exchange(senderId, 0 - amount, username);

                // cong tien
                await _walletRepository.Exchange(receiverId,  amount, username);

                //update status transaction

            


            }

            //_transactionRepository.CreateTransaction()


            return -1;
        }


    }
}
