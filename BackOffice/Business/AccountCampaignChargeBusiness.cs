using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business
{
    public class AccountCampaignChargeBusiness: IAccountCampaignChargeBusiness
    {
        private readonly ILogger<AccountCampaignChargeBusiness> _logger;
        private readonly IAccountCampaignChargeRepository _IAccountCampaignChargeRepository;

        public AccountCampaignChargeBusiness(ILoggerFactory _loggerFactory, IAccountCampaignChargeRepository __IAccountCampaignChargeRepository)
        {
            _logger = _loggerFactory.CreateLogger<AccountCampaignChargeBusiness>();

            _IAccountCampaignChargeRepository = __IAccountCampaignChargeRepository;
        }


        public List<AccountCampaignChargeViewModel> GetByAccountID(int id)
        {
            var list = _IAccountCampaignChargeRepository.ListAll().Where(a => a.AccountId == id);

            return list.Select(a => new AccountCampaignChargeViewModel(a)).ToList();

        }
    }
}
