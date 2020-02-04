using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
    public interface IWalletService
    {
        Task<long> GetAmount(AuthViewModel auth);
        Task<long> GetAmount(EntityType entityType, int entityId);
    }
}
