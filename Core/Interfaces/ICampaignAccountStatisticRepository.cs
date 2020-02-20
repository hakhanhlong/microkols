using Common;
using Core.Entities;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICampaignAccountStatisticRepository : IRepository<CampaignAccountStatistic>, IAsyncRepository<CampaignAccountStatistic>
    {
        Task Update(int campaignaccountid, int countlike, int countshare, int countcomment);
    }
}
