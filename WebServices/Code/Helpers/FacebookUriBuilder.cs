using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.Code.Helpers
{
    public class FacebookUriBuilder
    {
        public static string GetPostsAndCounting(string userid)
        {

            var fields = new List<string>() {
                "picture","message","created_time","link"
            };

            var reactionTypes = new string[] { "LIKE", "LOVE", "WOW", "HAHA", "SAD", "ANGRY" };

            foreach (var reactionType in reactionTypes)
            {
                fields.Add($"reactions.type({reactionType}).limit(0).summary(1).as({reactionType.ToLower()})");
            }
            var fieldstr = string.Join(',', fields);

            return $"{userid}/posts?limit=1000&since=1514764800&fields={fieldstr}";
        }
        public static string GetPosts(string userid)
        {
            //10211187493683462/posts?limit=100&since=1514764800&fields=picture,message,created_time,link
            var time = 1514764800; // 1/1/2018
            return $"{userid}/posts?limit=1000&since=1514764800&fields=full_picture,message,created_time,link,shares,comments.summary(1),likes.limit(0).summary(1),reactions.summary(1)";
        }

        public static string GetPostCounting(List<int> postids)
        {
            /*
             ?ids=10211187493683462_10211300194060901,10211187493683462_10211222612881420&fields=
reactions.type(LIKE).limit(0).summary(1).as(like), 
reactions.type(WOW).limit(0).summary(1).as(wow),
reactions.type(SAD).limit(0).summary(1).as(sad),
reactions.type(LOVE).limit(0).summary(1).as(love),
reactions.type(ANGRY).limit(0).summary(1).as(angry),
reactions.type(HAHA).limit(0).summary(1).as(haha)             
             */
            var idstr = string.Join(',', postids);

            var fields = new List<string>();

            var reactionTypes = new string[] { "LIKE", "LOVE", "WOW", "HAHA", "SAD", "ANGRY" };

            foreach (var reactionType in reactionTypes)
            {
                fields.Add($"reactions.type({reactionType}).limit(0).summary(1).as({reactionType.ToLower()})");
            }
            var fieldstr = string.Join(',', fields);
            return $"?id={idstr}&&fields={fieldstr}";

        }
    }
}
