using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.ViewModels;

namespace Website.Interfaces
{
    public interface IWalletService
    {
        Task<long> GetAmount(AuthViewModel auth);
        Task<long> GetAmount(EntityType entityType, int entityId);
    }
}
