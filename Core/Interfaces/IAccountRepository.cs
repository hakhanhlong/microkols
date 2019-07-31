using Core.Entities;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAccountRepository : IRepository<Account>, IAsyncRepository<Account>
    {
        Task<List<int>> GetActivedAccountIds();
        Task<Account> GetActivedAccount(int id);
        Task<Account> GetActivedAccount(string email);
        Task<Account> GetAccount(string email);
        IQueryable<Account> Query(IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender, 
            int? cityid, int? agestart, int? ageend, IEnumerable<int> ignoreIds, int min, int max);

        int CountAll();
    }
}
