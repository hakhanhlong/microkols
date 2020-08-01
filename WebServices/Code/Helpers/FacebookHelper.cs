using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Code.Helpers
{
    public interface IFacebookHelper
    {
        string GetAvatarUrl(string facebookid);
        Task<LoginProviderViewModel> GetLoginProviderAsync(string accessToken);
        Task<List<AccountFbPostViewModel>> GetPosts(string accessToken, string fid, long since = 1514764800, int limit = 10000);
        Task<AccountFbInfoViewModel> GetInfo(string accessToken, string fid);
        Task<FbExtendTokenViewModel> GetExtendToken(string accessToken);

        Task<dynamic> GetPermissions(string accessToken);

    }
    public class FacebookHelper : IFacebookHelper
    {
        private readonly IFacebookClient _facebookClient;
        private readonly SharedOptions _options;

        public FacebookHelper(IFacebookClient facebookClient, IOptionsMonitor<SharedOptions> optionsAccessor)
        {
            _options = optionsAccessor.CurrentValue;
            _facebookClient = facebookClient;
        }



        public string GetAvatarUrl(string facebookid)
        {
            return $"http://graph.facebook.com/{facebookid}/picture?type=large";
        }

        public async Task<LoginProviderViewModel> GetLoginProviderAsync(string accessToken)
        {
            try
            {
                var user = await _facebookClient.GetAsync<dynamic>(accessToken, "me", "fields=email,name");
                var id = (string)user.id;
                var email = (string)user.email;
                if (string.IsNullOrEmpty(email))
                {
                    email = $"{id}@facebook.com";
                }
                return new LoginProviderViewModel()
                {
                    Email = email,
                    ProviderId = id,
                    Name = (string)user.name,
                    Provider = Core.Entities.AccountProviderNames.Facebook,
                    AccessToken = accessToken,
                    Image = GetAvatarUrl((string)user.id),

                };
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<List<AccountFbPostViewModel>> GetPosts(string accessToken, string fid, long since = 1514764800, int limit = 10000)
        {

            //var p = $"limit={limit}&since={since}&fields=full_picture,permalink_url,message,created_time,link,shares,comments.summary(1),likes.limit(0).summary(1),reactions.summary(1)";            
            //var postResult = await _facebookClient.GetAsync<dynamic>(accessToken, $"{fid}/posts", p);
            var p = $"limit={limit}&since={since}&"+"fields=message,comments.limit(10).summary(true){message,from,likes.limit(0).summary(true)},reactions.limit(0).summary(true),created_time,link,permalink_url,full_picture";
            var postResult = await _facebookClient.GetAsync<dynamic>(accessToken, $"me/feed", p);

            var result = new List<AccountFbPostViewModel>();
            if (postResult != null)
            {
                var posts = (JArray)postResult["data"];
                foreach (var post in posts)
                {
                    result.Add(new AccountFbPostViewModel((dynamic)post));
                }

            }

            return result;
        }

        public async Task<AccountFbInfoViewModel> GetInfo(string accessToken, string fid)
        {

            var postResult = await _facebookClient.GetAsync<dynamic>(accessToken, $"{fid}", $"fields=id,friends.limit(0),link");            
            return new AccountFbInfoViewModel(postResult);

        }

        public async Task<dynamic> GetPermissions(string accessToken)
        {

            var postResult = await _facebookClient.GetAsync<dynamic>(accessToken, "/me/permissions");

            return postResult;
        }







        public async Task<FbExtendTokenViewModel> GetExtendToken(string accessToken)
        {
            return await _facebookClient.GetAsync<FbExtendTokenViewModel>(null, $"oauth/access_token", $"grant_type=fb_exchange_token&client_id={_options.FacebookAppId}&client_secret={_options.FacebookAppSecret}&fb_exchange_token={accessToken}");
        }

    }
}
