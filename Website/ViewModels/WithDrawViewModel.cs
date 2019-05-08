using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
    public class WithDrawViewModel
    {
        [Display(Name="Số tiền")]
        [Required]
        public int Amount { get; set; }

        [Display(Name="Thông tin tài khoản nhận tiền")]
        public string Note { get; set; }
    }


    public class OrderWithDrawViewModel
    {
        public int OrderId { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        public int Amount { get; set; }
    }
}
