using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.ViewModels
{
    public class ListAccountFbPostViewModel
    {
        public ListAccountFbPostViewModel()
        {

        }
        public ListAccountFbPostViewModel(IEnumerable<AccountFbPost> posts, int page, int pagesize, int total)
        {
            Posts = AccountFbPostViewModel.GetList(posts);
            Pager = new PagerViewModel(page, pagesize, total);
        }
        public List<AccountFbPostViewModel> Posts { get; set; }
        public PagerViewModel Pager { get; set; }
    }
    public class AccountFbInfoViewModel
    {
        public AccountFbInfoViewModel()
        {

        }
        public AccountFbInfoViewModel(dynamic obj)
        {
            try { Link = (string)obj.link; } catch { }

            try { Email = (string)obj.email; } catch { }


            try { FriendsCount = (int)obj.friends.summary.total_count; } catch { }
        }
        public string Link { get; set; }

        public string Email { get; set; }

        public int FriendsCount { get; set; }
    }
    public class AccountFbPostViewModel
    {

        public AccountFbPostViewModel(dynamic obj)
        {
            Id = 0;
            AccountId = 0;
            try { Picture = (string)obj.full_picture; } catch { }
            try { Message = (string)obj.message; } catch { }
            try { Link = (string)obj.link; } catch { }
            try { PostTime = (DateTime)obj.created_time; } catch { }
            try { ShareCount = (int)obj.shares.count; } catch { }
            try { LikeCount = (int)obj.reactions.summary.total_count; } catch { }
            try { CommentCount = (int)obj.comments.summary.total_count; } catch { }
            try { PostId = (string)obj.id; } catch { }
            try { Permalink = (string)obj.permalink_url; } catch { }

        }
        public AccountFbPostViewModel(AccountFbPost accountFbPost)
        {
            Id = accountFbPost.Id;
            AccountId = accountFbPost.AccountId;
            Picture = accountFbPost.Picture;
            Message = accountFbPost.Message;
            Link = accountFbPost.Link;
            PostId = accountFbPost.PostId;
            PostTime = accountFbPost.PostTime;
            ShareCount = accountFbPost.ShareCount;
            LikeCount = accountFbPost.LikeCount;
            CommentCount = accountFbPost.CommentCount;
            Permalink = accountFbPost.Permalink;
        }

        public static List<AccountFbPostViewModel> GetList(IEnumerable<AccountFbPost> posts)
        {
            return posts.Select(m => new AccountFbPostViewModel(m)).ToList();
        }


        public int Id { get; set; }
        public int AccountId { get; set; }

        public string Picture { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public string Permalink { get; set; }
        public string PostId { get; set; }
        public string PostId2
        {
            get
            {
                if (!string.IsNullOrEmpty(PostId))
                {
                    var arr = PostId.Split('_');
                    if(arr.Length== 2)
                    {
                        return arr[1];
                    }
                }

                return "";
            }
        }
        public DateTime PostTime { get; set; }

        public int ShareCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
