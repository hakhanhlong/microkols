using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.ViewModels;

namespace Website.Code.Helpers
{
    public interface IFacebookHelper
    {
        string GetAvatarUrl(string facebookid);
        Task<LoginProviderViewModel> GetLoginProviderAsync(string accessToken);
        Task<List<AccountFbPostViewModel>> GetPosts(string accessToken, string fid, long since = 1514764800, int limit = 10000);
        Task<FbExtendTokenViewModel> GetExtendToken(string accessToken);

    }
    public class FacebookHelper : IFacebookHelper
    {
        private readonly IFacebookClient _facebookClient;
        private readonly AppOptions _options;

        public FacebookHelper(IFacebookClient facebookClient, IOptionsMonitor<AppOptions> optionsAccessor)
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
            var user = await _facebookClient.GetAsync<dynamic>(accessToken, "me", "fields=email,name");

            return new LoginProviderViewModel()
            {
                Email = (string)user.email,
                ProviderId = (string)user.id,
                Name = (string)user.name,
                Provider = Core.Entities.AccountProviderNames.Facebook,
                AccessToken = accessToken,
                
            };
        }

        public async Task<List<AccountFbPostViewModel>> GetPosts(string accessToken, string fid, long since = 1514764800, int limit = 10000)
        {

            var postResult = await _facebookClient.GetAsync<dynamic>(accessToken, $"{fid}/posts", $"limit={limit}&since={since}&fields=full_picture,message,created_time,link,shares,comments.summary(1),likes.limit(0).summary(1),reactions.summary(1)");
            var result = new List<AccountFbPostViewModel>();
            var posts = (JArray)postResult["data"];
            foreach (var post in posts)
            {
                result.Add(new AccountFbPostViewModel((dynamic)post));
            }

            return result;
        }

        public async Task<FbExtendTokenViewModel> GetExtendToken(string accessToken)
        {
            return await _facebookClient.GetAsync<FbExtendTokenViewModel>(null, $"oauth/access_token", $"grant_type=fb_exchange_token&client_id={_options.FacebookAppId}&client_secret={_options.FacebookAppSecret}&fb_exchange_token={accessToken}");
        }

    }
}
