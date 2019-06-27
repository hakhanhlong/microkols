using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Agency : BaseEntityWithDate
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string TaxIdNumber { get; set; }

        public bool Deleted { get; set; }
        public bool Actived { get; set; }


        private List<Campaign> _Campaign = new List<Campaign>();
        public IEnumerable<Campaign> Campaign => _Campaign.AsReadOnly();
        
    }
}
