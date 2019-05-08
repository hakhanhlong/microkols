using Common;
using Common.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SettingRepository : EfRepository<Setting>, ISettingRepository
    {
     
        public SettingRepository(AppDbContext dbContext) : base(dbContext)
        {
            
        }
        public  async Task<Core.Models.SettingModel> GetSetting()
        {
            var settings = await _dbContext.Setting.ToListAsync();
            return new Core.Models.SettingModel(settings);
        }

      }
}
