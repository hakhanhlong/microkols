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
using WebServices.Code;
using WebServices.Code.Helpers;
using WebServices.Interfaces;
using WebServices.ViewModels;

namespace WebServices.Jobs
{

    public class FacebookJob : IFacebookJob
    {
        private readonly ILogger<FacebookJob> _logger;
        private readonly IFacebookHelper _facebookHelper;
        private readonly SharedOptions _options;
        private readonly IAccountService _accountService;
        private readonly ICampaignService _campaignService;
        private readonly ICampaignAccountStatisticRepository _campaignAccountStatisticRepository;
        public FacebookJob(ILoggerFactory loggerFactory, IFacebookHelper facebookHelper,
             IAccountService accountService, ICampaignAccountStatisticRepository campaignAccountStatisticRepository,
             ICampaignService campaignService,
            IOptionsMonitor<SharedOptions> optionsAccessor)
        {
            _logger = loggerFactory.CreateLogger<FacebookJob>();
            _accountService = accountService;
            _facebookHelper = facebookHelper;
            _options = optionsAccessor.CurrentValue;
            _campaignService = campaignService;
            _campaignAccountStatisticRepository = campaignAccountStatisticRepository;
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
                var since = type == 1 ? DateTime.Now.AddMonths(-6).ToUnixTime() : DateTime.Now.AddMonths(-2).ToUnixTime();
                //var since = DateTime.Now.AddDays(-5).ToUnixTime();
                // chi lay 1000 bai`


               var fbPosts = await _facebookHelper.GetPosts(accountProvider.AccessToken, accountProvider.ProviderId, since);

                if (fbPosts == null || fbPosts.Count == 0)
                {
                    return;
                }

               
                if (type == 2)
                {

                    var campaignAccounts = await _campaignService.GetListCampaignByAccount(accountid, 0, string.Empty, 1, fbPosts.Count);

                    foreach (var campaign in campaignAccounts.Campaigns)
                    {
                        AccountFbPostViewModel fbPost = null;
                        var refurl = campaign.CampaignAccount.RefUrl;
                        var refid = campaign.CampaignAccount.RefId;

                        // chỉ check facebook post của người đã đồng ý tham gia chiến dịch
                        if ((campaign.CampaignAccount.Status == CampaignAccountStatus.Confirmed ||
                            campaign.CampaignAccount.Status == CampaignAccountStatus.ApprovedContent ||
                            campaign.CampaignAccount.Status == CampaignAccountStatus.UpdatedContent) 
                            && campaign.Status == CampaignStatus.Started)
                        {
                            //if (string.IsNullOrEmpty(refid) && !string.IsNullOrEmpty(refurl))
                            if (!string.IsNullOrEmpty(refurl))
                            {
                                // truong hop influencer tụ cập nhật link kết quả 
                                fbPost = fbPosts.Where(m => !string.IsNullOrEmpty(m.PostId2) && refurl.Contains(m.PostId2)).FirstOrDefault();
                                if (fbPost == null)
                                {
                                    //trường hợp link user cập nhật ko khớp với link lấy từ fb
                                    fbPost = fbPosts.Where(m => campaign.CampaignAccount.RefUrl.Contains(m.Link)).FirstOrDefault();
                                }
                                if (fbPost == null)
                                {
                                    //truong hop link lấy từ fb ko khớp với link user
                                    fbPost = fbPosts.Where(m => m.Link.Contains(campaign.CampaignAccount.RefUrl)).FirstOrDefault();
                                }


                                //check bên permalink
                                if (fbPost == null)
                                {
                                    
                                    fbPost = fbPosts.Where(m => campaign.CampaignAccount.RefUrl.Contains(m.Permalink)).FirstOrDefault();
                                }
                                if (fbPost == null)
                                {
                                    
                                    fbPost = fbPosts.Where(m => m.Permalink.Contains(campaign.CampaignAccount.RefUrl)).FirstOrDefault();
                                }

                                if (fbPost == null)
                                {
                                    
                                    fbPost = fbPosts.Where(m => m.Permalink == campaign.CampaignAccount.RefUrl).FirstOrDefault();
                                }

                                //check link with campaign data field
                                if (fbPost == null)
                                {
                                    if (!string.IsNullOrEmpty(campaign.Data))
                                    {
                                        fbPost = fbPosts.Where(m => m.Link == campaign.Data || m.Link.Contains(campaign.Data)).FirstOrDefault();
                                    }
                                    
                                }

                                //#######################################################################################################
                                try
                                {
                                    string fbpostid = string.Empty;
                                    if (campaign.CampaignAccount.RefUrl.Contains("posts/"))
                                    {
                                        string[] strPosts = campaign.CampaignAccount.RefUrl.Split(new string[] { "posts/" }, StringSplitOptions.None);
                                        if (strPosts.Count() > 1)
                                        {
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(strPosts[1]))
                                                {
                                                    fbpostid = strPosts[1];
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(fbpostid))
                                    {
                                        if (fbPost == null)
                                        {
                                            fbPost = fbPosts.Where(m => m.Permalink.Contains(fbpostid)).FirstOrDefault();
                                        }
                                    }

                                }
                                catch { }
                          

                                //#######################################################################################################

                                if (fbPost != null)
                                {
                                    string msg = string.Empty;
                                    if (!string.IsNullOrEmpty(campaign.Data)) //kiểm tra xem link yêu cầu chia sẻ user có chia sẻ đúng link ko
                                    {
                                        
                                        if ((campaign.Data != fbPost.Link || !campaign.Data.Contains(fbPost.Link)) &&
                                            (campaign.HrefCompare != fbPost.Link || !campaign.HrefCompare.Contains(fbPost.Link)))
                                        {
                                            msg = $"Cần xác minh thực hiện chiến dịch! Link chia sẻ không đúng";
                                        }
                                        else
                                        {
                                            // Xử lý khi link cần chia sẻ đã được chia sẻ đúng
                                            msg = $"Cần xác minh thực hiện chiến dịch! Link chia sẻ đúng";

                                        }
                                        
                                    }
                                    else
                                    {
                                        msg = $"Cần xác minh thực hiện chiến dịch!";                                        

                                    }

                                    var campaignAccountId = await _campaignService.UpdateCampaignAccountRef(accountid, 
                                        new ViewModels.UpdateCampaignAccountRefViewModel()
                                    {
                                        CampaignId = campaign.Id,
                                        CampaignType = campaign.Type,                                        
                                        RefId = fbPost.PostId,
                                        RefUrl = fbPost.Link,
                                        Note = msg
                                        }, username);             
                                    
                                    if (campaignAccountId > 0)
                                    {
                                        await _campaignAccountStatisticRepository.Update(campaignAccountId, fbPost.LikeCount, fbPost.ShareCount, fbPost.CommentCount);
                                    }

                                }


                            }                                                    
                        }                         
                    }
                }
                //update thong tin like,share,comment
                foreach (var fbPost in fbPosts)
                {
                    if (!string.IsNullOrEmpty(fbPost.PostId))
                    {
                        await _accountService.UpdateFbPost(accountid, fbPost, username);
                    }
                }



            }
        }

        public async Task UpdateFbPost()
        {
            var accountIds = await _accountService.GetActivedAccountIds();

            foreach (var accountId in accountIds)
            {
                BackgroundJob.Enqueue<IFacebookJob>(m => m.UpdateFbPost(accountId, SharedConstants.USERNAME, 1));
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
            if (accountid == 5)
            {

            }
            var accountProvider = await _accountService.GetAccountProviderByAccount(accountid, AccountProviderNames.Facebook);
            if (accountProvider != null)
            {

                var info = await _facebookHelper.GetInfo(accountProvider.AccessToken, accountProvider.ProviderId);
                if (info != null)
                {
                    await _accountService.UpdateAccountProviderInfo(accountProvider.Id, "", info.FriendsCount, "bot", info.Email);
                }
            }
        }

        #endregion
    }
}
