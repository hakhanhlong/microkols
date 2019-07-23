using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class PayoutExport: BaseEntity
    {
        
        public DateTime StartDateExport { get; set; }
        public DateTime EndDateExport { get; set; }

        public AccountType AccountType { get; set; }

        public bool IsExport { get; set; }

        public bool IsUpdateWallet { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }
    }
}
