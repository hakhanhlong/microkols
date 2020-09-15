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
    public class PayoutExportService: IPayoutExportService
    {
        private IPayoutExportRepository _IPayoutExportRepository;
        public PayoutExportService(IPayoutExportRepository __IPayoutExportRepository) {
            _IPayoutExportRepository = __IPayoutExportRepository;
        }

        public PayoutExportViewModel GetPayoutExport(DateTime StartDate, DateTime EndDate, AccountType Type)
        {
            var paypoutExport =  _IPayoutExportRepository.GetPayoutExport(StartDate, EndDate, Type);
            return new PayoutExportViewModel(paypoutExport);

        }

        public bool IsExist(DateTime StartDate, DateTime EndDate, AccountType Type)
        {
            return _IPayoutExportRepository.IsExist(StartDate, EndDate, Type);
        }

        public async Task<ListPayoutExportViewModel> ListPayoutExport(AccountType[] type, int pageindex, int pagesize)
        {
            var filter = new PayoutExportSpecification(type);
            var listing = await _IPayoutExportRepository.ListPagedAsync(filter, "", pageindex, pagesize);
            int count = await _IPayoutExportRepository.CountAsync(filter);

            return new ListPayoutExportViewModel()
            {
                PayoutExport = listing.Select(s => new PayoutExportViewModel(s)).ToList(),
                Pager = new PagerViewModel()
                {
                    Page = pageindex,
                    PageSize = pagesize,
                    Total = count
                }

            }; 
        }
    }
}
