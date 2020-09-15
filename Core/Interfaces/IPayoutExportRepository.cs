using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IPayoutExportRepository: IRepository<PayoutExport>, IAsyncRepository<PayoutExport>
    {
        PayoutExport GetPayoutExport(DateTime StartDate, DateTime EndDate, AccountType Type);
        

        bool IsExist(DateTime StartDate, DateTime EndDate, AccountType Type);
    }
}
