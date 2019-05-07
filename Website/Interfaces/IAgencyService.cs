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
        Task<int> CreateAgency(CreateAgencyViewModel model);

        Task<AuthViewModel> GetAuth(LoginViewModel model);
        Task<AuthViewModel> GetAuth(int id);

        Task<UpdateAgencyViewModel> GetUpdateAgency(int id);
        Task<bool> UpdateAgency(int id, UpdateAgencyViewModel model, string username);

    }
}
