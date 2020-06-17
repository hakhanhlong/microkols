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
    public class AgencyRepository : EfRepository<Agency>, IAgencyRepository
    {

        public AgencyRepository(AppDbContext dbContext) : base(dbContext)
        {}

        public async Task<Agency> GetActivedAgency(int id)
        {
            return await _dbContext.Agency.FirstOrDefaultAsync(m => m.Id == id && m.Actived);
        }

        public async Task<Agency> GetBySaltAgency(string salt)
        {
            return await _dbContext.Agency.FirstOrDefaultAsync(m => m.Salt == salt);
        }


        public async Task<Agency> GetActivedAgency(string username)
        {
            return await _dbContext.Agency.FirstOrDefaultAsync(m => m.Username == username && m.Actived);
        }

        public async Task<Agency> GetAgency(string username)
        {
            return await _dbContext.Agency.FirstOrDefaultAsync(m => m.Username == username);
        }


        public int CountAll()
        {
            return _dbContext.Agency.Count();
        }
    }
}
