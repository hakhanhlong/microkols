using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Agency : BaseEntityWithMeta
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        private List<Campaign> _Campaign = new List<Campaign>();
        public IEnumerable<Campaign> Campaign => _Campaign.AsReadOnly();
        
    }
}
