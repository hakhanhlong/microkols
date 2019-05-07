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
        {

        }

        public async Task<Agency> GetPublishedAgency(int id)
        {
            return await _dbContext.Agency.FirstOrDefaultAsync(m => m.Id == id && m.Published);
        }
        public async Task<Agency> GetPublishedAgency(string username)
        {
            return await _dbContext.Agency.FirstOrDefaultAsync(m => m.Username == username && m.Published);
        }

        public async Task<Agency> GetAgency(string username)
        {
            return await _dbContext.Agency.FirstOrDefaultAsync(m => m.Username == username);
        }
    }
}
