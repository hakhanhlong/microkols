using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class AccountFbPost : BaseEntityWithDate
    {
        public int AccountId { get; set; }
        public string Picture { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public string PostId { get; set; }

        public DateTime PostTime { get; set; }

        public int ShareCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
