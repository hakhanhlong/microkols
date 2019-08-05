using BackOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business.Interfaces
{
    public interface IAccountCampaignChargeBusiness
    {
        List<AccountCampaignChargeViewModel> GetByAccountID(int id);
    }
}
