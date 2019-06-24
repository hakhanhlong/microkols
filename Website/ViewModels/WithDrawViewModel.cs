using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
    public class WithDrawViewModel
    {
        [Display(Name="Số tiền rút")]
        [Required(ErrorMessage ="Hãy nhập {0}")]
        public long Amount { get; set; }

        [Display(Name="Ghi chú")]
        public string Note { get; set; }
    }


  
}
