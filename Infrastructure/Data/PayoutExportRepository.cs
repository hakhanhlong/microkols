using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Infrastructure.Data
{
    public class PayoutExportRepository: EfRepository<PayoutExport>, IPayoutExportRepository
    {
        public PayoutExportRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public PayoutExport GetPayoutExport(DateTime StartDate, DateTime EndDate, AccountType Type)
        {
            return _dbContext.PayoutExport.Where(pe=>pe.StartDateExport == StartDate && pe.EndDateExport == EndDate && pe.AccountType == Type).FirstOrDefault();
        }

        public bool IsExist(DateTime StartDate, DateTime EndDate, AccountType Type)
        {
            return _dbContext.PayoutExport.Where(pe => pe.StartDateExport == StartDate && pe.EndDateExport == EndDate && pe.AccountType == Type).Count() > 0?true:false;
        }


    }
}
