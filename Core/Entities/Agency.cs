using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        public AgencyType? Type { get; set; }


        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string CheckVerified { get; set; }

        private List<Campaign> _Campaign = new List<Campaign>();
        public IEnumerable<Campaign> Campaign => _Campaign.AsReadOnly();
        
    }

    public  enum AgencyType
    {

        [Display(Name ="Doanh nghiệp")]
        DoanhNghiep = 1,
        [Display(Name ="Hộ kinh doanh/Cá thể kinh doanh")]
        HoKinhDoanh = 2
    }
}
