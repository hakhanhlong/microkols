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
    public class AccountBusiness: IAccountBusiness
    {

        private readonly ILogger<AccountBusiness> _logger;
        private readonly IAccountRepository _IAccountRepository;

        public AccountBusiness(ILoggerFactory _loggerFactory, IAccountRepository __IAccountRepository)
        {
            _logger = _loggerFactory.CreateLogger<AccountBusiness>();

            _IAccountRepository = __IAccountRepository;
        }



        public async Task<AccountViewModel> GetAccount(int id)
        {
            var account = await _IAccountRepository.GetByIdAsync(id);

            return GetAccountViewModel(account);
        }

        private AccountViewModel GetAccountViewModel(Account account)
        {
            return (account == null) ? null : new AccountViewModel(account);
        }

        public ListAccountViewModel GetListAccount(int pageindex, int pagesize)
        {
            var accounts = _IAccountRepository.ListPaging("DateModified_desc", pageindex, pagesize);

            var total = _IAccountRepository.CountAll();


            return new ListAccountViewModel()
            {
                Accounts  = accounts.Select(a => new AccountViewModel(a)).ToList(),

                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }
    }
}
