using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.ViewModels
{
    public class QnAViewModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }


    }

    public class QnAGroupViewModel
    {
        public string Title { get; set; }
        public List<QnAViewModel> QnAs { get; set; }


    }
}
