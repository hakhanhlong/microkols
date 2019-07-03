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
        private readonly IAgencyBusiness _IAgencyBusiness;
        private readonly IAccountBusiness _IAccountBusiness;

        public WalletBusiness(ILoggerFactory _loggerFactory, IWalletRepository __IWalletRepository,
            IAgencyBusiness __IAgencyBusiness, IAccountBusiness __IAccountBusiness)
        {
            _logger = _loggerFactory.CreateLogger<WalletBusiness>();

            _IWalletRepository = __IWalletRepository;
            _IAgencyBusiness = __IAgencyBusiness;
            _IAccountBusiness = __IAccountBusiness;
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

        public WalletViewModel Get(int id)
        {
            var wallet = _IWalletRepository.GetById(id);
            WalletViewModel _WalletViewModel = null;
            if (wallet!= null)
            {
                _WalletViewModel = new WalletViewModel(wallet);
                if (wallet.EntityType == Core.Entities.EntityType.Account)
                {
                    _WalletViewModel.Name = _IAccountBusiness.GetAccount(wallet.EntityId).Result.Name;
                }
                else if(wallet.EntityType == Core.Entities.EntityType.Agency)
                {
                    _WalletViewModel.Name = _IAgencyBusiness.GetAgency(wallet.EntityId).Result.Name;
                }
            }

            return _WalletViewModel;
        }
    }
}
