using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Website.ViewModels
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
            Roles = new List<string>() { "Account" };
        }
        public AuthViewModel(Agency agency)
        {
            Id = agency.Id;
            Name = agency.Name;
            Username = agency.Username;
            Type = EntityType.Agency;
            Roles = new List<string>() { "Agency" };
        }
        public List<string> Roles { get; set; } = new List<string>();
        public int Id { get; set; }
        public EntityType Type { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }

        public List<Claim> GetClaims()
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, Id.ToString()));
            claims.Add(new Claim(ClaimTypes.GivenName, Name));
            claims.Add(new Claim(ClaimTypes.Name, Username));
            foreach (var role in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        public static AuthViewModel GetModel(ClaimsPrincipal principal)
        {
            var id = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
            var username = principal.FindFirst(ClaimTypes.Name).Value;
            var name = principal.FindFirst(ClaimTypes.GivenName).Value;
            return new AuthViewModel()
            {
                Id = id,
                Name = name,
                Username = username,
                Roles = new List<string>() // not get roles 
            };
        }





    }
}
