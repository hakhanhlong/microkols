using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Jobs
{
  
    public class FacebookJob
    { 
        private readonly ILogger<FacebookJob> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMemoryCache _cache;
        private readonly IAsyncRepository<CampaignAccount> _campaignAccountRepository;


        public FacebookJob(ILoggerFactory loggerFactory, IMemoryCache cache, ICampaignRepository campaignRepository,
            IAccountRepository accountRepository,
             INotificationRepository notificationRepository, IAsyncRepository<CampaignAccount> campaignAccountRepository)
        {
            _logger = loggerFactory.CreateLogger<FacebookJob>();
            _notificationRepository = notificationRepository;
            _cache = cache;
            _campaignRepository = campaignRepository;
            _accountRepository = accountRepository;
            _campaignAccountRepository = campaignAccountRepository;
        }


    }
}
