using Common.Extensions;
using Common.Helpers;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
    public class AgencyViewModel : UpdateAgencyViewModel
    {
        public AgencyViewModel()
        {

        }
        public AgencyViewModel(Agency agency) : base(agency)
        {
            Id = agency.Id;
        }
        public int Id { get; set; }
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
        }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên doanh nghiệp")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string Image { get; set; }
    }

    public class RegisterAgencyViewModel : UpdateAgencyViewModel
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
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên đăng nhập")]
        [Remote("VerifyAgencyUsername", "Auth", ErrorMessage = "{0} đã tồn tại")]
        public string Username { get; set; }


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
                Description = Description,
                Image = Image,
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
