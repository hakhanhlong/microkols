using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class WalletFilterSpecification: BaseSpecification<Wallet>
    {
        public WalletFilterSpecification(EntityType? entitytype)
           : base(w => (!entitytype.HasValue || w.EntityType == entitytype.Value))
        {
            
        }


        public WalletFilterSpecification(int walletid)
           : base(w => w.Id == walletid)
        {}
    }
}
