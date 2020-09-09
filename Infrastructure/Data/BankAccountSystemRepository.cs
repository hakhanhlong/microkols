using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class BankAccountSystemRepository : EfRepository<BankAccountSystem>, IBankAccountSystemRepository
    {
        public BankAccountSystemRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
