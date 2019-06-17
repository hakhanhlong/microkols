using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Hangfire;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Code;
using Website.Code.Helpers;
using Website.Interfaces;

namespace Website.Jobs
{

    public class FacebookJob : IFacebookJob
    {
        private readonly ILogger<FacebookJob> _logger;
        private readonly IFacebookHelper _facebookHelper;
        private readonly AppOptions _options;
        private readonly IAccountService _accountService;
        public FacebookJob(ILoggerFactory loggerFactory, IFacebookHelper facebookHelper,
             IAccountService accountService,
            IOptionsMonitor<AppOptions> optionsAccessor)
        {
            _logger = loggerFactory.CreateLogger<FacebookJob>();
            _accountService = accountService;
            _facebookHelper = facebookHelper;
            _options = optionsAccessor.CurrentValue;
        }

        #region ExtendAccessToken
        public async Task ExtendAccessToken()
        {
            var accountProviders = await _accountService.GetAccountProvidersByExpiredToken(AccountProviderNames.Facebook);

            foreach (var accountProvider in accountProviders)
            {
                BackgroundJob.Enqueue<IFacebookJob>(m => m.ExtendAccessToken(accountProvider.Id, accountProvider.AccessToken));
            }
        }

        public async Task ExtendAccessToken(int id, string tokenExpired)
        {
            var accessToken = await _facebookHelper.GetExtendToken(tokenExpired);
            if (accessToken != null)
            {
                await _accountService.UpdateAccountProvidersAccessToken(id, accessToken.AccessToken, accessToken.ExpiresIn);
            }
        }

        #endregion


        #region Update Facebook Post

        public async Task UpdateFbPost(int accountid, string username)
        {
            
            var accountProvider = await _accountService.GetAccountProviderByAccount(accountid, AccountProviderNames.Facebook);
            if (accountProvider != null)
            {

                // chi lay 1000 bai`
                var fbPosts = await _facebookHelper.GetPosts(accountProvider.AccessToken, accountProvider.ProviderId);

                foreach (var fbPost in fbPosts)
                {
                    if (!string.IsNullOrEmpty(fbPost.PostId))
                    {
                        await _accountService.UpdateFbPost(accountid, fbPost, username);
                    }

                }
            }


        }
        #endregion


    }
}
