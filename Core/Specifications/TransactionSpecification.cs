using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{

    public class TransactionSpecification : BaseSpecification<Transaction>
    {
       
        public TransactionSpecification(int id)
           : base(i => i.Id == id)
        {

        }

        public TransactionSpecification(TransactionType type): base(t=>t.Type == type) { }

        public TransactionSpecification(TransactionStatus status) : base(t => t.Status== status) { }

    }

    public class TransactionByCampaignSpecification : BaseSpecification<Transaction>
    {

        public TransactionByCampaignSpecification(int campaignid)
           : base(i => i.RefId == campaignid)
        {

        }
    }
}
