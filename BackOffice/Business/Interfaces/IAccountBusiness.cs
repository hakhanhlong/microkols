using BackOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business.Interfaces
{
    public interface IAccountBusiness
    {
        Task<AccountViewModel> GetAccount(int id);
        ListAccountViewModel GetListAccount(int pageindex, int pagesize);
    }
}
