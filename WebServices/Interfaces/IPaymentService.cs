using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
   public  interface IPaymentService
    {
        Task<PaymentResultViewModel> CreateAgencyPayment(int agencyId, CreateCampaignPaymentViewModel model, string username);
        Task<PaymentResultViewModel> CreatePaybackCampaignAccount(int campaignid, int accountid, string username);
        Task<bool> VerifyPaybackCampaignAccount(int campaignid);

        Task<bool> IsExistPaymentServiceCashBack(int agencyId, int campaignid);

        Task<int> CreateTransactionCampaignAccountPayback(int campaignid, int accountid, string username);

        Task<PaymentResultViewModel> Pay(int transactionid, int senderId, int receiverId, long amount, TransactionType transactionType, string note, string username);
    }
}
