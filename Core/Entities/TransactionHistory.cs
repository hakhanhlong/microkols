using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class TransactionHistory : BaseEntity
    {
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public long Amount { get; set; }
        public long Balance { get; set; }
        public string Note { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
