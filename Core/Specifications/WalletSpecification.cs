using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class WalletSpecification : BaseSpecification<Wallet>
    {
        public WalletSpecification(EntityType? entitytype)
           : base(w => (!entitytype.HasValue || w.EntityType == entitytype.Value))
        {
            
        }


        public WalletSpecification(int walletid)
           : base(w => w.Id == walletid)
        {}
    }
}
