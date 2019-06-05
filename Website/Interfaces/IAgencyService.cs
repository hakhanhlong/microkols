using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.ViewModels;

namespace Website.Interfaces
{
    public interface IAgencyService
    {

        Task<AgencyViewModel> GetAgency(int id);
        Task<int> Register(RegisterAgencyViewModel model);

        Task<AuthViewModel> GetAuth(AgencyLoginViewModel model);
        Task<AuthViewModel> GetAuth(int id);

        Task<UpdateAgencyViewModel> GetUpdateAgency(int id);
        Task<bool> UpdateAgency(int id, UpdateAgencyViewModel model, string username);

        Task<bool> ChangePassword(int id, ChangePasswordViewModel model, string username);

        Task<bool> VerifyUsername(string username);

    }
}
