using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.CommonHelpers
{
    public class AppHelpers
    {
   

        public static string RESOURCE_SERVER { get; set; }
        public static string RESOURCE_PATH { get; set; }
        public static string ResourceTempDir { get; set; }

        public string GetImageUrl(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith("http")) return path;
                return $"{RESOURCE_SERVER}/{path}";
            }
            return string.Empty;


        }
    }
}
