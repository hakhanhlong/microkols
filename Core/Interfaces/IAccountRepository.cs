using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAccountRepository : IRepository<Account>, IAsyncRepository<Account>
    {
        Task<Account> GetActivedAccount(int id);
        Task<Account> GetActivedAccount(string email);
        Task<Account> GetAccount(string email);
    }
}
