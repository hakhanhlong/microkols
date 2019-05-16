using Common;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IWalletRepository : IRepository<Wallet>, IAsyncRepository<Wallet>
    {
        Task<int> GetSystemId();
        Task<int> GetWalletId(EntityType entityType, int entityId);
        Task<long> GetBalance(EntityType entityType, int entityId);
        Task<Wallet> GetWallet(EntityType entityType, int entityId);
        Task<long> Exchange(int walletid, long value, string username);
        Task<int> CreateWallet(EntityType entityType, int entityId);
    }
}
