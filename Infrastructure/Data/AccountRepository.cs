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

        public int CountAll()
        {
            return _dbContext.Account.Count();
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



        public async Task<IQueryable<Account>> Query(IEnumerable<int> categoryid, Gender? gender, int? cityid, int? agestart, int? ageend)
        {


            var query = _dbContext.Account.Where(m => m.Actived);
            if (categoryid.Any())
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
                var dt = new DateTime(1, 1, DateTime.Now.Year);
                var dtstart = dt.AddYears(0 - ageend.Value);
                var dtend = dt.AddYears(0 - agestart.Value);

                query = query.Where(m => m.Birthday.HasValue && m.Birthday.Value >= dtstart && m.Birthday.Value <= dtend);
            }

            return query;

        }

    }
}
