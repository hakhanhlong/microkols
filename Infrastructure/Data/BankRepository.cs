using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class BankRepository : EfRepository<Bank>, IBankRepository
    {
        public BankRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
