using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Code
{
    public class AppOptions
    {
     
        public string GoogleApiKey { get; set; }
        public string GoogleAppId { get; set; }
        public string FacebookAppId { get; set; }

        public string FacebookAppToken { get; set; }

        public string ResourceServer { get; set; }
        public string ResourcePath { get; set; }
        public string ResourceTempDir { get; set; }

        public string GetImageUrl(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                return $"{ResourceServer}/{path}";
            }
            return string.Empty;

            
        }

        public SmtpOptions SmtpServer { get; set; }
    }
    public class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
