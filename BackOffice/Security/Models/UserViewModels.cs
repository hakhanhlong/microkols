using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Security.Models
{
    public class LoginModel
    {
        [Required]
        [UIHint("email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }

    }

    public class ChangePasswordModel
    {
        [Required]
        [UIHint("password")]
        public string OldPassword { get; set; }


        [Required]
        [UIHint("password")]
        public string NewPassword { get; set; }

        [Required]
        [UIHint("password")]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

    }
}
