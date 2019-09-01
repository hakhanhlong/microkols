using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{
    public class ListAgencyViewModel
    {
        public List<AgencyViewModel> Agencies { get; set; }
        public PagerViewModel Pager { get; set; }
    }

    public class AgencyViewModel 
    {
        public AgencyViewModel(){}

        public AgencyViewModel(Agency agency)
        {
            Id = agency.Id;
            DateCreated = agency.DateCreated;
            DateModified = agency.DateModified;
            UserCreated = agency.UserCreated;
            UserModified = agency.UserModified;
            Actived = agency.Actived;
            Deleted = agency.Deleted;
            Name = agency.Name;
            Description = agency.Description;
            Image = agency.Image;
            TaxNumber = agency.TaxIdNumber;
            Phone = agency.Phone;
            Email = agency.Email;
            UserName = agency.Username;
            Address = agency.Address;
        }

        public string UserName { get; set; }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public string UserCreated { get; set; }
        public string UserModified { get; set; }
        public bool Actived { get; set; }

        public bool Deleted { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }
        public string Image { get; set; }

        public string TaxNumber { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }




    }


}
