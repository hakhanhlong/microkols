using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.ViewModels
{
    public class WithDrawViewModel
    {
        [Display(Name="Số tiền rút")]
        [Required(ErrorMessage ="Hãy nhập {0}")]
        public long Amount { get; set; }

        [Display(Name="Ghi chú")]
        public string Note { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tài khoản")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Số tài khoản")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Ngân hàng")]
        public string Bank { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Chi nhánh")]
        public string Branch { get; set; }
    }


  
}
