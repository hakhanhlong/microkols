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
    public class WalletBusiness: IWalletBusiness
    {

        private readonly ILogger<WalletBusiness> _logger;
        private readonly IWalletRepository _IWalletRepository;

        public WalletBusiness(ILoggerFactory _loggerFactory, IWalletRepository __IWalletRepository)
        {
            _logger = _loggerFactory.CreateLogger<WalletBusiness>();

            _IWalletRepository = __IWalletRepository;
        }


        public ListWalletViewModel GetListWallet(int pageindex, int pagesize)
        {
            var wallets = _IWalletRepository.ListPaging("DateModified_desc", pageindex, pagesize);

            var total = _IWalletRepository.CountAll();


            return new ListWalletViewModel()
            {
                Wallets = wallets.Select(a => new WalletViewModel(a)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }
    }
}
