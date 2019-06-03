using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
    public class LoginViewModel
    {
     
        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không đúng")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }


        [Display(Name = "Ghi nhớ")]
        public bool Remember { get; set; }
    }

    public class AgencyLoginViewModel
    {

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }


        [Display(Name = "Ghi nhớ")]
        public bool Remember { get; set; }
    }

    public class LoginProviderViewModel
    {
        public string Provider { get; set; }

        public string ProviderId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
      
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên đăng nhập hoặc Email")]
        public string Username { get; set; }
    }

    public class ForgotPasswordResultViewModel
    {
        public string NewPassword { get; set; }
        public string Email { get; set; }
    }

    public class RegisterViewModel
    {


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Họ và tên", Prompt = "Họ Tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Email", Prompt = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Hãy nhập đúng định dạng Email")]
        [Remote("VerifyEmail", "Auth", ErrorMessage = "{0} đã tồn tại")]
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
