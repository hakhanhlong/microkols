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

        public TransactionSpecification(TransactionType type, TransactionStatus status) : base(t => t.Status == status && t.Type == type) { }

        public TransactionSpecification(TransactionType type, TransactionStatus status, DateTime startDate, DateTime endDate) : base(t => t.Status == status && t.Type == type 
        && t.DateCreated >= startDate && t.DateCreated <= endDate) { }

        public TransactionSpecification(DateTime? startDate, DateTime? endDate) : base(t => (!startDate.HasValue || t.DateCreated.Date >= startDate.Value.Date) && (!endDate.HasValue || t.DateCreated.Date <= endDate.Value.Date))
        { }

        public TransactionSpecification(int senderid, int receiverid, TransactionType type, int RefId) : 
            base(t => t.SenderId == senderid && t.ReceiverId == receiverid && t.Type == type && t.RefId == RefId && t.Status!= TransactionStatus.Error) { }

        public TransactionSpecification(int senderid, int receiverid) : base(t => t.SenderId == senderid || t.ReceiverId == receiverid) { }

    }

    public class TransactionByCampaignSpecification : BaseSpecification<Transaction>
    {

        public TransactionByCampaignSpecification(int campaignid)
           : base(i => i.RefId == campaignid)
        {

        }
    }
}
