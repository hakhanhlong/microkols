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

    }
    public class FacebookHelper : IFacebookHelper
    {
        private readonly IFacebookClient _facebookClient;

        public FacebookHelper(IFacebookClient facebookClient)
        {
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
                Provider = "Facebook"
            };
        }
    }
}
