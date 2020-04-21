using BackOffice.Models;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business.Interfaces
{
    public interface IWalletBusiness
    {
        ListWalletViewModel GetListWallet(int pageindex, int pagesize);
        Task<ListWalletViewModel> GetListWallet(EntityType? type, int pageindex, int pagesize);

        WalletViewModel Get(int id);

        ListWalletViewModel Search(string keyword, EntityType entityType, AccountType type, int pageindex, int pagesize);

    }
}
