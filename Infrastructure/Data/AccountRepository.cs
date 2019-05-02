using Common;
using Common.Helpers;
using Core;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AccountRepository : EfRepository<Account>, IAccountRepository
    {

        public AccountRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Account> GetActivedAccount(int id)
        {
            return await _dbContext.Account.FirstOrDefaultAsync(m => m.Id == id && m.Actived);
        }
        public async Task<Account> GetActivedAccount(string email)
        {
            return await _dbContext.Account.FirstOrDefaultAsync(m => m.Email == email && m.Actived);
        }

        public async Task<Account> GetAccount(string email)
        {
            return await _dbContext.Account.FirstOrDefaultAsync(m => m.Email == email);
        }
    }
}
