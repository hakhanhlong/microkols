using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{
    public class ListMicroKolViewModel
    {
        public List<MicroKolViewModel> Campaigns { get; set; }
        public PagerViewModel Pager { get; set; }
    }

    public class MicroKolViewModel
    {
        public MicroKolViewModel() { }


        public MicroKolViewModel(Account a) { }
    }
}
