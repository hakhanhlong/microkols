using Common;
using Common.Extensions;
using Common.Helpers;
using Core;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Extensions;
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

        public int CountAll()
        {
            return _dbContext.Account.Count();
        }

        public async Task<Account> GetActivedAccount(int id)
        {
            return await _dbContext.Account.FirstOrDefaultAsync(m => m.Id == id && m.Actived);
        }

        public async Task<List<int>> GetActivedAccountIds()
        {
            return await _dbContext.Account.Where(m => m.Actived).Select(m => m.Id).ToListAsync();
        }
        public async Task<Account> GetActivedAccount(string email)
        {
            return await _dbContext.Account.FirstOrDefaultAsync(m => m.Email == email && m.Actived);
        }

        public async Task<Account> GetAccount(string email)
        {
            return await _dbContext.Account.FirstOrDefaultAsync(m => m.Email == email);
        }



        public IQueryable<Account> Query(IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender,
            int? cityid, int? agestart, int? ageend, IEnumerable<int> ignoreIds, int min, int max)
        {


            var query = _dbContext.Account.Where(m => m.Actived);
            if (ignoreIds != null && ignoreIds.Any())
            {
                query = query.Where(m => !ignoreIds.Contains(m.Id));
            }
            if (accountTypes != null && accountTypes.Any())
            {
                query = query.Where(m => accountTypes.Contains(m.Type));
            }
            if (categoryid != null && categoryid.Any())
            {
                query = query.Where(m => m.AccountCategory.Any(n => categoryid.Contains(n.CategoryId)));

            }

            if (gender.HasValue)
            {
                query = query.Where(m => m.Gender == gender.Value);
            }

            if (cityid.HasValue)
            {
                query = query.Where(m => m.CityId == cityid);
            }

            if (agestart.HasValue && ageend.HasValue)
            {
                var dt = new DateTime(DateTime.Now.Year, 1, 1);
                var dtstart = dt.AddYears(0 - ageend.Value);
                var dtend = dt.AddYears(0 - agestart.Value);

                query = query.Where(m => m.Birthday.HasValue && m.Birthday.Value >= dtstart && m.Birthday.Value <= dtend);
            }

          
            return query;

        }

    }
}
