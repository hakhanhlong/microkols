using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServices.ViewModels
{
    public class PayoutExportViewModel
    {

        public PayoutExportViewModel(PayoutExport pe) {
            Id = pe.Id;
            StartDateExport = pe.StartDateExport;
            EndDateExport = pe.EndDateExport;
            AccountType = pe.AccountType;
            IsExport = pe.IsExport;
            IsUpdateWallet = pe.IsUpdateWallet;
            CreatedDate = pe.CreatedDate;
            PayoutExportFileDate = pe.PayoutExportFileDate;
            PayoutPayDate = pe.PayoutPayDate;
            CreatedUser = pe.CreatedUser;

        }

        public int Id { get; set; }

        public DateTime StartDateExport { get; set; }
        public DateTime EndDateExport { get; set; }

        public AccountType AccountType { get; set; }

        public bool IsExport { get; set; }

        public bool IsUpdateWallet { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? PayoutExportFileDate { get; set; }

        public DateTime? PayoutPayDate { get; set; }


        public string CreatedUser { get; set; }

    }

    public class ListPayoutExportViewModel
    {
        public List<PayoutExportViewModel> PayoutExport { get; set; }
        public PagerViewModel Pager { get; set; }
    }

}
