using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AccountFbPostRepository : EfRepository<AccountFbPost>, IAccountFbPostRepository
    {
        public AccountFbPostRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<string> GetAccessToken(int accountid)
        {
            var accountProvider = await _dbContext.AccountProvider.FirstOrDefaultAsync(m => m.Provider == AccountProviderNames.Facebook && m.AccountId == accountid);
            if(accountProvider!= null)
            {
                return accountProvider.AccessToken;
            }
            return string.Empty;
        }

        public async Task<AccountCountingModel> GetAccountCounting(int accountid)
        {

            var accountProvider = await _dbContext.AccountProvider.FirstOrDefaultAsync(m => m.Provider == AccountProviderNames.Facebook && m.AccountId == accountid);
            var followersCount = 0;
            var friendsCount = 0;
            var fblink = string.Empty;
            var fbid = string.Empty;
            if(accountProvider != null)
            {
                followersCount = accountProvider.FollowersCount ?? 0;
                friendsCount = accountProvider.FriendsCount ?? 0;
                fblink = accountProvider.Link;
                fbid = accountProvider.ProviderId;
            }
            var queryFbPost = _dbContext.AccountFbPost.Where(m => m.AccountId == accountid);

            var avgLikeCount = await queryFbPost.Select(m => m.LikeCount).DefaultIfEmpty(0).AverageAsync();
            var avgShareCount = await queryFbPost.Select(m => m.ShareCount).DefaultIfEmpty(0).AverageAsync();
            var avgCommentCount = await queryFbPost.Select(m => m.CommentCount).DefaultIfEmpty(0).AverageAsync();

            return new AccountCountingModel()
            {
                AvgCommentCount = avgCommentCount,
                AvgLikeCount = avgLikeCount,
                AvgShareCount = avgShareCount,
                FollowersCount = followersCount,
                FriendsCount = friendsCount,
                AccountId = accountid,
                FacebookLink = fblink,
                FacebookId = fbid
            };
        }


    }
}
