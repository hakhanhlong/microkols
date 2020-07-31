using BackOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business.Interfaces
{
    public interface IAgencyBusiness
    {
        Task<AgencyViewModel> GetAgency(int id);
        ListAgencyViewModel GetListAgency(int pageindex, int pagesize);

        ListAgencyViewModel Search(string kw, int pageindex, int pagesize);

        Task UpdateAgency(AgencyViewModel model);

        bool Active(int id);
        bool UnActive(int id);


    }
}
