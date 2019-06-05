using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business
{
    public class MicroKolBusiness : IMicroKolBusiness
    {
        private readonly ILogger<MicroKolBusiness> _logger;
        private readonly IAccountRepository _IAccountRepository;

        public MicroKolBusiness(ILoggerFactory _loggerFactory, IAccountRepository __IAccountRepository)
        {
            _logger = _loggerFactory.CreateLogger<MicroKolBusiness>();
            _IAccountRepository = __IAccountRepository;
        }

        public ListMicroKolViewModel GetListMicroKol(int pageindex, int pagesize)
        {
            return new ListMicroKolViewModel();
        }
    }
}
