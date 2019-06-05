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
            Published = agency.Published;
            Deleted = agency.Deleted;
            Name = agency.Name;
            Description = agency.Description;
            Image = agency.Image;
        }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public string UserCreated { get; set; }
        public string UserModified { get; set; }
        public bool Published { get; set; }

        public bool Deleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

    }


}
