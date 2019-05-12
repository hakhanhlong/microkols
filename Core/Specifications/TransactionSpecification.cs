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
    }

    public class TransactionByCampaignSpecification : BaseSpecification<Transaction>
    {

        public TransactionByCampaignSpecification(int campaignid)
           : base(i => i.RefId == campaignid)
        {

        }
    }
}
