using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{
    public class AccountCountingModel
    {
        public int AccountId { get; set; }
        public double AvgShareCount { get; set; }
        public double AvgLikeCount { get; set; }
        public double AvgCommentCount { get; set; }
        public int FriendsCount { get; set; }
        public int FollowersCount { get; set; }
    }
}
