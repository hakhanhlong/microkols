using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.ViewModels;

namespace Website.Interfaces
{
   public  interface IPaymentService
    {
        Task<PaymentResultViewModel> CreatePayment(int agencyId, CreateCampaignPaymentViewModel model, string username);

    }
}
