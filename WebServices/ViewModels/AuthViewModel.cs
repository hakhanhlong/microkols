using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Extensions;
using System.Security.Principal;

namespace WebServices.ViewModels
{
    public class AuthViewModel
    {
        public AuthViewModel()
        {
        }
        public AuthViewModel(Account account)
        {
            Id = account.Id;
            Name = account.Name;
            Username = account.Email;
            Type = EntityType.Account;
            Avatar = account.Avatar;
            Roles = new List<string>() { "Account" };
            //AccountActived = account.Actived?"Actived":"UnActived";
        }
        public AuthViewModel(Agency agency)
        {
            Id = agency.Id;
            Name = agency.Name;
            Username = agency.Username;
            Type = EntityType.Agency;
            Avatar = agency.Image;
            Roles = new List<string>() { "Agency" };
            AgencyActived = agency.Actived;
        }
        public List<string> Roles { get; set; } = new List<string>();
        public int Id { get; set; }
        public EntityType Type { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        
        public bool AgencyActived { get; set; }

        public List<Claim> GetClaims()
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, Id.ToString()));
            claims.Add(new Claim(ClaimTypes.GivenName, Name));
            claims.Add(new Claim(ClaimTypes.Name, Username));
            claims.Add(new Claim("Avatar", Avatar));
            claims.Add(new Claim("Type", Type.ToString()));
            //claims.Add(new Claim("AccountActived", AccountActived));

            foreach (var role in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        public static AuthViewModel GetModel(ClaimsPrincipal principal)
        {
            int id = 0;
            Int32.TryParse(principal.FindFirst(ClaimTypes.NameIdentifier).Value, out id);
            var username = principal.FindFirst(ClaimTypes.Name).Value;
            var name = principal.FindFirst(ClaimTypes.GivenName).Value;

            var avatar = principal.FindFirst("Avatar").Value;
            var type = principal.FindFirst("Type").Value;            

            return new AuthViewModel()
            {
                Id = id,
                Name = name,
                Username = username,
                Avatar = avatar,
                Type = type.ToEnum<EntityType>(),                
                Roles = new List<string>() // not get roles ,
                
            };
        }

    }
}
