using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{


    public class ListTransactionHistoryViewModel
    {
        public List<TransactionHistoryViewModel> TransactionHistories { get; set; }
        public PagerViewModel Pager { get; set; }
    }

   

    public class TransactionHistoryViewModel
    {

        public TransactionHistoryViewModel(){}

        public TransactionHistoryViewModel(TransactionHistory t) {

            Id = t.Id;
            Transaction = new TransactionViewModel(t.Transaction);
            Wallet = new WalletViewModel(t.Wallet);
            Amount = t.Amount;
            Balance = t.Balance;
            Note = t.Note;
            DateCreated = t.DateCreated;


        }

        public int Id { get; set; }
        public TransactionViewModel Transaction { get; set; }

        public WalletViewModel Wallet { get; set; }

        public long Amount { get; set; }

        public long Balance { get; set; }

        public string Note { get; set; }

        public DateTime DateCreated { get; set; }


    }
}
