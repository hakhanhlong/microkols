using Core.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace WebServices.ViewModels
{
    public class RegisterViewModel
    {


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Họ và tên", Prompt = "Họ Tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Email", Prompt = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Hãy nhập đúng định dạng Email")]
        //[Remote("VerifyEmail", "Auth", ErrorMessage = "{0} đã tồn tại")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Mật khẩu", Prompt = "Mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "{0} Có độ dài từ {2} - {1} ký tự.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Xác nhận mật khẩu", Prompt = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "{0} Có độ dài từ {2} - {1} ký tự.", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }





    }

}
