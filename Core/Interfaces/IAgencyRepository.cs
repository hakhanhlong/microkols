using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAgencyRepository : IRepository<Agency>, IAsyncRepository<Agency>
    {
        Task<Agency> GetActivedAgency(int id);
        Task<Agency> GetActivedAgency(string username);
        Task<Agency> GetAgency(string username);
    }
}
