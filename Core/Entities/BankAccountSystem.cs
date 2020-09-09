using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class BankAccountSystem: BaseEntity
    {
        public string BankAccountName { get; set; }

        public string BankAccountNumber { get; set; }

        public string BankBranch { get; set; }

        public string BankName { get; set; }

        public string BankCode { get; set; }

        public string BankImageUrl { get; set; }

        public bool IsActive { get; set; }


    }
}
