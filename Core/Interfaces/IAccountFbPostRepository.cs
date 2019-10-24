using Core.Entities;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAccountFbPostRepository : IRepository<AccountFbPost>, IAsyncRepository<AccountFbPost>
    {
        Task<string> GetAccessToken(int acountid);
        Task<AccountCountingModel> GetAccountCounting(int accountid);
        Task<List<AccountFbPost>> GetAccountFbPostsByHasCampaign(int accountid, int page, int pagesize);
    }
}
