using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackOffice.Areas.Access.Models
{
    public class UserRoleViewModel
    {
        [Required]
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}