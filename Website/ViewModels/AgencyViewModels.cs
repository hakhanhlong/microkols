using Common.Extensions;
using Common.Helpers;
using Core.Entities;
using Microsoft.AspNetCore.Http;
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
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class CreateAgencyViewModel : UpdateAgencyViewModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
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
                Published = false,
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
