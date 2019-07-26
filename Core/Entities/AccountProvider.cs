using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class AccountProvider : BaseEntity
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expired { get; set; }

        public AccountProviderNames Provider { get; set; }
        public string ProviderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
        public int? FriendsCount { get; set; }
        public int? FollowersCount { get; set; }

    }

    public enum AccountProviderNames
    {
        Facebook,
        Google
    }



}
