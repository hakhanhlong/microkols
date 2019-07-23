using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{
    public class PayoutExportViewModel
    {

        public PayoutExportViewModel() { }

        public PayoutExportViewModel(PayoutExport p)
        {
            Id = p.Id;
            StartDateExport = p.StartDateExport;
            EndDateExport = p.EndDateExport;
            AccountType = p.AccountType;
            IsExport = p.IsExport;
            IsUpdateWallet = p.IsUpdateWallet;
            CreatedDate = p.CreatedDate;
            CreatedUser = p.CreatedUser;
        }

        public int Id { get; set; }

        public DateTime StartDateExport { get; set; }
        public DateTime EndDateExport { get; set; }

        public AccountType AccountType { get; set; }

        public bool IsExport { get; set; }

        public bool IsUpdateWallet { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }
    }
}
