using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
    public class AccountCountingViewModel: AccountCountingModel
    {
        public AccountCountingViewModel()
        {

        }
        public AccountCountingViewModel(AccountCountingModel model)
        {
            if(model!= null)
            {
                AvgShareCount = Math.Round(model.AvgShareCount,2);
                AvgLikeCount = Math.Round(model.AvgLikeCount, 2);
                AvgCommentCount = Math.Round(model.AvgCommentCount, 2);
                FriendsCount = model.FriendsCount;
                FollowersCount = model.FollowersCount;
                FacebookLink = model.FacebookLink;

            }
        }
    }
}
