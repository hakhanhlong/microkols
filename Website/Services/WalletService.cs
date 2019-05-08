
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.ViewModels;

namespace Website.Services
{
    public class WalletService : BaseService, IWalletService
    {

        private readonly IWalletRepository _walletRepository;

        public WalletService(
        IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }


        public async Task<long> GetAmount(EntityType entityType, int entityId)
        {
            return await _walletRepository.GetBalance(entityType, entityId);

        }
        public async Task<long> GetAmount(AuthViewModel auth)
        {
            return await GetAmount(auth.Type, auth.Id);

        }

    }
}
