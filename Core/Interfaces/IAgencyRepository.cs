using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAgencyRepository : IRepository<Agency>, IAsyncRepository<Agency>
    {
        Task<Agency> GetPublishedAgency(int id);
        Task<Agency> GetPublishedAgency(string username);
        Task<Agency> GetAgency(string username);
    }
}
