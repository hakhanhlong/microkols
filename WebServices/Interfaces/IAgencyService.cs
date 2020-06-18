using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
    public interface IAgencyService
    {

        Task<AgencyViewModel> GetAgency(int id);

        Task<AgencyViewModel> GetAgency(string salt);
        Task<int> VerifyEmail(int id);
        Task<int> Register(RegisterAgencyViewModel model);

        Task<AuthViewModel> GetAuth(AgencyLoginViewModel model);
        Task<AuthViewModel> GetAuth(int id);

        Task<UpdateAgencyViewModel> GetUpdateAgency(int id);
        Task<bool> UpdateAgency(int id, UpdateAgencyViewModel model, string username);

        Task<bool> ChangePassword(int id, ChangePasswordViewModel model, string username);

        Task<bool> VerifyUsername(string username);

        Task<bool> VerifyUsername(string username, string name);


        Task<AgencyViewModel> GetAgencyById(int id);

    }
}
