using Common.Extensions;
using Common.Helpers;
using Core.Entities;


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.ViewModels
{
    public class AgencyViewModel : UpdateAgencyViewModel
    {
        public AgencyViewModel()
        {

        }
        public AgencyViewModel(Agency agency) : base(agency)
        {
            Id = agency.Id;
            Salt = agency.Salt;
        }
        public int Id { get; set; }
        public string Salt { get; set; }
    }


    public class UpdateAgencyViewModel
    {
        public UpdateAgencyViewModel()
        {

        }
        public UpdateAgencyViewModel(Agency entity)
        {
            Name = entity.Name;
            Description = entity.Description;
            Image = entity.Image;
            TaxIdNumber = entity.TaxIdNumber;
            Type = entity.Type;
            Address = entity.Address;
            Email = entity.Email;
            Phone = entity.Phone;
        }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên doanh nghiệp", Prompt = "Tên doanh nghiệp")]
        public string Name { get; set; }

        [Display(Name = "Lĩnh vực hoạt động")]
        public string Description { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string Image { get; set; }


        [Display(Name = "Mã số thuế")]
        public string TaxIdNumber { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Loại Tài khoản")]
        public AgencyType? Type { get; set; }


        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }


        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

    }

    public class RegisterAgencyViewModel 
    {
        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Mật khẩu", Prompt = "Mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "{0} Có độ dài từ {2} - {1} ký tự.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Xác nhận mật khẩu", Prompt = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "{0} Có độ dài từ {2} - {1} ký tự.", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "{0} và {1} không trùng nhau")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Email đăng nhập", Prompt = "Email đăng nhập")]
        [EmailAddress(ErrorMessage = "{0} không đúng")]
        //[Remote("VerifyAgencyUsername", "Auth", ErrorMessage = "{0} đã tồn tại hoặc không phải Email doanh nghiệp")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên doanh nghiệp", Prompt = "Tên doanh nghiệp")]
        public string Name { get; set; }

        public Agency GetEntity()
        {
            var salt = SecurityHelper.GenerateSalt();
            var pwhash = SecurityHelper.HashPassword(salt, Password);

            return new Agency()
            {
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Deleted = false,
                Actived = false,
                Description = string.Empty,
                Image = string.Empty,
                Name = Name,
                Salt = salt,
                Password = pwhash,
                UserCreated = Username,
                Username = Username,
                UserModified = Username
            };
        }
    }


}
