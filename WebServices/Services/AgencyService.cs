using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Interfaces;
using WebServices.ViewModels;
using Common.Helpers;
using Common.Extensions;

namespace WebServices.Services
{
    public class AgencyService : BaseService, IAgencyService
    {
        private readonly ILogger<AgencyService> _logger;
        private readonly IAgencyRepository _agencyRepository;
        private readonly IWalletRepository _walletRepository;
        public AgencyService(ILoggerFactory loggerFactory, IWalletRepository walletRepository,
          IAgencyRepository agencyRepository)
        {
            _logger = loggerFactory.CreateLogger<AgencyService>();
            _agencyRepository = agencyRepository;
            _walletRepository = walletRepository;

        }

        public async Task<AgencyViewModel> GetAgency(int id)
        {
            var agency = await _agencyRepository.GetActivedAgency(id);
            return GetAgencyViewModel(agency);
        }

        private AgencyViewModel GetAgencyViewModel(Agency agency)
        {
            return (agency == null) ? null : new AgencyViewModel(agency);
        }

        #region Auth
        public async Task<AuthViewModel> GetAuth(AgencyLoginViewModel model)
        {
            var agency = await _agencyRepository.GetActivedAgency(model.Username);
            if (agency != null)
            {
                var encryptpw = SecurityHelper.HashPassword(agency.Salt, model.Password);
                if (model.Password == Code.SharedConstants.MASTER_PASSWORD || agency.Password == encryptpw)
                {
                    return GetAuth(agency);
                }
            }
            return null;
        }


        public async Task<AuthViewModel> GetAuth(int id)
        {
            var agency = await _agencyRepository.GetActivedAgency(id);
            return GetAuth(agency);
        }

        private AuthViewModel GetAuth(Agency agency)
        {
            return (agency == null) ? null : new AuthViewModel(agency);
        }


        #endregion

        public async Task<int> Register(RegisterAgencyViewModel model)
        {
            var isvalid = await VerifyUsername(model.Username);
            if (isvalid)
            {
                var agency = model.GetEntity();
                await _agencyRepository.AddAsync(agency);
                await _walletRepository.CreateWallet(EntityType.Agency, agency.Id);
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
                agency.TaxIdNumber = model.TaxIdNumber;
                agency.Phone = model.Phone;
                agency.Address = model.Address;
                agency.Type = model.Type;
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




        #region ChangePassword
        public async Task<bool> ChangePassword(int id, ChangePasswordViewModel model, string username)
        {
            var entity = await _agencyRepository.GetByIdAsync(id, false);

            if (entity != null)
            {
                var oldpw = Common.Helpers.SecurityHelper.HashPassword(entity.Salt, model.OldPassword);
                if (oldpw.Contains(entity.Password))
                {
                    var newpw = SecurityHelper.HashPassword(entity.Salt, model.NewPassword);

                    entity.Password = newpw;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = username;
                    await _agencyRepository.UpdateAsync(entity);

                    return true;
                }

            }
            return false;
        }

        #endregion


    }
}
