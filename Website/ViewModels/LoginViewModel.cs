﻿using Core.Entities;
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

  
}
