using Common.Extensions;
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
        private readonly ICampaignService _campaignService;
        public FacebookJob(ILoggerFactory loggerFactory, IFacebookHelper facebookHelper,
             IAccountService accountService,
             ICampaignService campaignService,
            IOptionsMonitor<AppOptions> optionsAccessor)
        {
            _logger = loggerFactory.CreateLogger<FacebookJob>();
            _accountService = accountService;
            _facebookHelper = facebookHelper;
            _options = optionsAccessor.CurrentValue;
            _campaignService = campaignService;
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

        public async Task UpdateFbPost(int accountid, string username, int type = 1)
        {

            var accountProvider = await _accountService.GetAccountProviderByAccount(accountid, AccountProviderNames.Facebook);
            if (accountProvider != null)
            {
                var since = type == 1 ? new DateTime(2018, 1, 1).ToUnixTime() : DateTime.Now.AddMonths(-1).ToUnixTime();
                // chi lay 1000 bai`
                var fbPosts = await _facebookHelper.GetPosts(accountProvider.AccessToken, accountProvider.ProviderId, since);


                foreach (var fbPost in fbPosts)
                {
                    if (!string.IsNullOrEmpty(fbPost.PostId))
                    {
                        await _accountService.UpdateFbPost(accountid, fbPost, username);
                    }
                }


                if (type == 2)
                {
                    var listcampaigns = await _campaignService.GetListCampaignByAccount(accountid,0, string.Empty, 1, 100);
                    var campaigns = listcampaigns.Campaigns.Where(m => m.Status == CampaignStatus.Started && (m.Type == CampaignType.ShareContent || m.Type == CampaignType.ShareContentWithCaption || m.Type == CampaignType.ShareStreamUrl));


                    foreach (var campaign in campaigns)
                    {
                        var fbPost = fbPosts.Where(m => m.Link.Contains(campaign.Data)).FirstOrDefault();
                        if(fbPost!= null)
                        {

                            await _campaignService.UpdateCampaignAccountRef(accountid, new ViewModels.UpdateCampaignAccountRefViewModel()
                            {
                                CampaignId = campaign.Id,
                                CampaignType = campaign.Type,
                                Note = string.Empty,
                                RefId = fbPost.PostId,
                                RefUrl = fbPost.Permalink,

                            }, username);
                        }

                     
                    }

                }
            }
        }

        public async Task UpdateFbPost()
        {
            var accountIds = await _accountService.GetActivedAccountIds();

            foreach (var accountId in accountIds)
            {
                BackgroundJob.Enqueue<IFacebookJob>(m => m.UpdateFbPost(accountId, AppConstants.USERNAME, 2));
            }
        }
        #endregion


        #region Update Facebook Info
        public async Task UpdateFbInfo()
        {
            var accountIds = await _accountService.GetActivedAccountIds();

            foreach (var accountId in accountIds)
            {
                BackgroundJob.Enqueue<IFacebookJob>(m => m.UpdateFbInfo(accountId));
            }
        }

        public async Task UpdateFbInfo(int accountid)
        {
            if(accountid== 5)
            {

            }
            var accountProvider = await _accountService.GetAccountProviderByAccount(accountid, AccountProviderNames.Facebook);
            if (accountProvider != null)
            {

                var info = await _facebookHelper.GetInfo(accountProvider.AccessToken, accountProvider.ProviderId);
                if(info!= null)
                {
                    await _accountService.UpdateAccountProviderInfo(accountProvider.Id, info.Link, info.FriendsCount, "bot");
                }
            }
        }

        #endregion
    }
}
