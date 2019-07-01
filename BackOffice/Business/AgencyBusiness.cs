using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business
{
    public class AgencyBusiness: IAgencyBusiness
    {
        private readonly ILogger<AgencyBusiness> _logger;
        private readonly IAgencyRepository _IAgencyRepository;

        public AgencyBusiness(ILoggerFactory _loggerFactory, IAgencyRepository __IAgencyRepository) {
            _logger = _loggerFactory.CreateLogger<AgencyBusiness>();
            _IAgencyRepository = __IAgencyRepository;
        }


        public async Task<AgencyViewModel> GetAgency(int id)
        {
            var agency = await _IAgencyRepository.GetByIdAsync(id);
            return GetAgencyViewModel(agency);
        }

        private AgencyViewModel GetAgencyViewModel(Agency agency)
        {
            return (agency == null) ? null : new AgencyViewModel(agency);
        }

        public ListAgencyViewModel GetListAgency(int pageindex, int pagesize)
        {
            var agencies = _IAgencyRepository.ListPaging("DateModified_desc", pageindex, pagesize);
            var total = _IAgencyRepository.CountAll();


            return new ListAgencyViewModel()
            {
                Agencies = agencies.Select(a=> new AgencyViewModel(a)).ToList(),

                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }

        public bool Active(int id)
        {
            var agency = _IAgencyRepository.GetById(id);
            if (agency != null)
            {
                agency.Actived = true;
                _IAgencyRepository.Update(agency);

                return true;
            }
            return false;
        }

        public bool UnActive(int id)
        {
            var agency = _IAgencyRepository.GetById(id);
            if (agency != null)
            {
                agency.Actived = false;
                _IAgencyRepository.Update(agency);

                return true;
            }
            return false;
        }

    }
}
