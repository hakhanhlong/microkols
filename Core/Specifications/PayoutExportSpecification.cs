using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Core.Specifications
{
    public class PayoutExportSpecification : BaseSpecification<PayoutExport>
    {
        public PayoutExportSpecification(int id) : base(m => m.Id > 0)
        {
        }


        public PayoutExportSpecification(AccountType type) : base(m => m.AccountType  == type)
        {
        }

        public PayoutExportSpecification(AccountType []type) : base(m => type.Contains(m.AccountType))
        {
        }
    }
}
