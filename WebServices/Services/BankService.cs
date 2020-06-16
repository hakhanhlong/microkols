using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServices.Interfaces;
using WebServices.ViewModels;
using System.Linq;

namespace WebServices.Services
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _IBankRepository;
        public BankService(IBankRepository __IBankRepository) {
            _IBankRepository = __IBankRepository;
        }

        public async Task<List<BankViewModel>> ListAll()
        {            
            var list = await _IBankRepository.ListAllAsync();
            return list.Select(b => new BankViewModel(b)).ToList();
            
        }

        
    }
}
