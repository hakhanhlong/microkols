using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
    public interface IPayoutExportService
    {

        PayoutExportViewModel GetPayoutExport(DateTime StartDate, DateTime EndDate, AccountType Type);

        Task<PayoutExportViewModel> GetPayoutExport(int id);

        bool IsExist(DateTime StartDate, DateTime EndDate, AccountType Type);

        Task<ListPayoutExportViewModel> ListPayoutExport(AccountType[] Type, int pageindex, int pagesize);


    }
}
