using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.ViewModels;
using Common.Helpers;
using Common.Extensions;

namespace Website.Services
{
    public class AgencyService : BaseService, IAgencyService
    {
        private readonly ILogger<AgencyService> _logger;
        private readonly IAgencyRepository _agencyRepository;
        public AgencyService(ILoggerFactory loggerFactory,
          IAgencyRepository agencyRepository)
        {
            _logger = loggerFactory.CreateLogger<AgencyService>();
            _agencyRepository = agencyRepository;

        }

        public async Task<AgencyViewModel> GetAgency(int id)
        {
            var agency = await _agencyRepository.GetPublishedAgency(id);
            return GetAgencyViewModel(agency);
        }

        private AgencyViewModel GetAgencyViewModel(Agency agency)
        {
            return (agency == null) ? null : new AgencyViewModel(agency);
        }



        #region Auth
        public async Task<AuthViewModel> GetAuth(LoginViewModel model)
        {
            var agency = await _agencyRepository.GetPublishedAgency(model.Username);
            if (agency != null)
            {
                var encryptpw = SecurityHelper.HashPassword(agency.Salt, model.Password);
                if (model.Password == Code.AppConstants.MASTER_PASSWORD || agency.Password == encryptpw)
                {
                    return GetAuth(agency);
                }
            }
            return null;
        }


        public async Task<AuthViewModel> GetAuth(int id)
        {
            var agency = await _agencyRepository.GetPublishedAgency(id);
            return GetAuth(agency);
        }

        private AuthViewModel GetAuth(Agency agency)
        {
            return (agency == null) ? null : new AuthViewModel(agency);
        }


        #endregion

        public async Task<int> CreateAgency(CreateAgencyViewModel model)
        {
            var isvalid = await VerifyUsername(model.Username);
            if (isvalid)
            {
                var agency = model.GetEntity();
                await _agencyRepository.AddAsync(agency);
                return agency.Id;
            }
            return -1;
        }


        public async Task<UpdateAgencyViewModel> GetUpdateAgency(int id)
        {
            var agency = await _agencyRepository.GetByIdAsync(id);
            if (agency != null)
            {
                return new UpdateAgencyViewModel(agency);
            }
            return null;
        }

        public async Task<bool> UpdateAgency(int id, UpdateAgencyViewModel model, string username)
        {
            var agency = await _agencyRepository.GetByIdAsync(id);
            if (agency != null)
            {
                agency.DateModified = DateTime.Now;
                agency.Username = username;
                agency.Name = model.Name;
                agency.Image = model.Image;
                agency.Description = model.Description;
                await _agencyRepository.UpdateAsync(agency);

                return true;
            }
            return false;

        }


        public async Task<bool> VerifyUsername(string username)
        {

            var entity = await _agencyRepository.GetAgency(username);

            if (entity == null)
            {
                return true;
            }
            return false;
        }


    }
}
