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
    }
}
