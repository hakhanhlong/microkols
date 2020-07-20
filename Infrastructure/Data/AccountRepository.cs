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
            return await _dbContext.Account.FirstOrDefaultAsync(m => m.Id == id && m.Actived == true);
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

        public async Task<IQueryable<Account>> QueryMatchedAccountByCampiagn(int campaignid)
        {
            var query = _dbContext.Account.Where(m => m.Actived);

            var campaign = await _dbContext.Campaign.FindAsync(campaignid);
            if (campaign == null)
            {
                return query.Where(m => m.Id == 0);
            }
            var accountids = await _dbContext.CampaignAccount.Where(m => m.CampaignId == campaignid).Select(m => m.AccountId).ToListAsync();
            query = query.Where(m => !accountids.Contains(m.Id));

            var accountTypes = await _dbContext.CampaignAccountType.Where(m => m.CampaignId == campaignid).Select(m => m.AccountType).ToListAsync();
            query = query.Where(m => accountTypes.Contains(m.Type));

            /*
            //phan nay query sau - lam truoc logic
            var campaignOptions = await _dbContext.CampaignOption.Where(m => m.CampaignId == campaignid).ToListAsync();
            if (campaignOptions.Count > 0)
            {
                var categoryids = campaignOptions.Where(m => m.Name == CampaignOptionName.Category).Select(m=>  int.Parse(m.Value)).ToList();
                if (categoryids.Count > 0)
                {
                    query = query.Where(m => m.AccountCategory.Any(i => categoryids.Contains(i.CategoryId)));

                }
            }

            */


            return query;

        }


        public IQueryable<Account> Query(IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender,
           IEnumerable<int> cityid, int? agestart, int? ageend, IEnumerable<int> ignoreIds, int min, int max)
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

            if (cityid != null && cityid.Any())
            {
                query = query.Where(m => m.CityId.HasValue && cityid.Contains(m.CityId.Value));
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


        public async Task UpdateAccountCategory(int accountid, List<int> categoryid)
        {
            var accountCategories = await _dbContext.AccountCategory.Where(m => m.AccountId == accountid).ToListAsync();

            _dbContext.AccountCategory.RemoveRange(accountCategories);
            await _dbContext.SaveChangesAsync();


            if (categoryid != null && categoryid.Count > 0)
            {
                foreach (var catid in categoryid)
                {
                    await _dbContext.AccountCategory.AddAsync(new AccountCategory()
                    {
                        AccountId = accountid,
                        CategoryId = catid
                    });
                }
                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
