using BackOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business.Interfaces
{
    public interface IWalletBusiness
    {
        ListWalletViewModel GetListWallet(int pageindex, int pagesize);
        WalletViewModel Get(int id);


    }
}
