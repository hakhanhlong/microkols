using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Wallet : BaseEntity
    {
        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public int Balance { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }


        private List<TransactionHistory> _TransactionHistory = new List<TransactionHistory>();
        public IEnumerable<TransactionHistory> TransactionHistory => _TransactionHistory.AsReadOnly();
    }
   
}
