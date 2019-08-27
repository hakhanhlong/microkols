using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
    public class QnAViewModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }


    }

    public class QnAGroupViewModel
    {
        public string Title { get; set; }
        public List<QnAViewModel> QuAs { get; set; }


    }
}
