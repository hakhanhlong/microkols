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
    public class CampaignAccountStatisticRepository : EfRepository<CampaignAccountStatistic>, ICampaignAccountStatisticRepository
    {

        public CampaignAccountStatisticRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public async Task Update(int campaignaccountid, int countlike, int countshare,int countcomment)
        {
            var now = DateTime.Now;
            var dateStart = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var dateEnd = dateStart.AddDays(1).AddSeconds(-1);

            var entity = await _dbContext.CampaignAccountStatistic.Where(m => m.CampaignAccountId == campaignaccountid && m.Date >= dateStart && m.Date <= dateEnd).FirstOrDefaultAsync();

            if(entity!= null)
            {
                entity.CountComment += countcomment;
                entity.CountLike += countlike;
                entity.CountShare += countshare;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                entity = new CampaignAccountStatistic()
                {
                    CampaignAccountId = campaignaccountid,
                    Date = dateStart,
                    CountComment = countcomment,
                    CountLike = countlike,
                    CountShare = countshare
                };
                await _dbContext.CampaignAccountStatistic.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

            }
        }


    }
}
