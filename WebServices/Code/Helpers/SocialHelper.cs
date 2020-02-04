using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Code.Helpers
{
    public class SocialHelper
    {
        public static async Task<LoginProviderViewModel> VerifyGoogleTokenAsync(string token)
        {
            try
            {
                var query = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + token;
                var client = new HttpClient();

                string response = await client.GetStringAsync(query); // could also use GetStreamAsync and avoid conversion to Stream

                dynamic user = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);
                return new LoginProviderViewModel()
                {
                    Email = (string)user.email,
                    ProviderId = (string)user.sub,
                    Name = (string)user.name,
                    Provider = AccountProviderNames.Google,
                    AccessToken = token
                };
            }
            catch
            {
                return null;
            }

        }


        public static async Task<LoginProviderViewModel> VerifyFacebookTokenAsync(string token)
        {
            try
            {
                var query = $"https://graph.facebook.com/me?access_token={token}&fields=email,name";

                var client = new HttpClient();

                string response = await client.GetStringAsync(query); // could also use GetStreamAsync and avoid conversion to Stream

                dynamic user = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);
                return new LoginProviderViewModel()
                {
                    Email = (string)user.email,
                    ProviderId = (string)user.id,
                    Name = (string)user.name,
                    Provider = AccountProviderNames.Facebook,
                    AccessToken= token
                };
            }
            catch
            {
                return null;
            }

        }
    }
}
