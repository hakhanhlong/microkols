using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{


    public class ListTransactionViewModel
    {
        public List<TransactionViewModel> Transactions { get; set; }
        public PagerViewModel Pager { get; set; }

        public string keyword { get; set; }

        public TransactionStatus TransactionStatus  { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string SearchType { get; set; }

        public TransactionType? TransactionType { get; set; }


    }

    public class GroupTransactionViewModel
    {
        public int walletid { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }

        public AccountViewModel Account { get; set; }
        public bool IsCashOut { get; set; }

        public Wallet Wallet { get; set; }

    }

    public class TransactionViewModel
    {

        public TransactionViewModel(){}

        public TransactionViewModel(Transaction t) {
            Id = t.Id;
            DateCreated = t.DateCreated;
            DateModified = t.DateModified;
            UserCreated = t.UserCreated;
            UserModified = t.UserModified;
            Code = t.Code;
            SenderId = t.SenderId;
            ReceiverId = t.ReceiverId;
            Amount = t.Amount;
            Type = t.Type;
            Status = t.Status;
            Data = t.Data;
            RefId = t.RefId;
            RefData = t.RefData;
            Note = t.Note;
            AdminNote = t.AdminNote;
            CashoutDate = t.CashoutDate;
            IsCashOut = t.IsCashOut;
        }

        public static List<TransactionViewModel> GetList(IEnumerable<Transaction> transactions)
        {
            return transactions.Select(m => new TransactionViewModel(m)).ToList();
        }

        public WalletViewModel Wallet { get; set; }

        public WalletViewModel WalletReceiver { get; set; }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
        public string Code { get; set; }

        public DateTime? CashoutDate { get; set; }
        public bool? IsCashOut { get; set; }


        public int SenderId { get; set; }
        public string SenderName { get; set; }

        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }

        public long Amount { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public string Data { get; set; }
        public int? RefId { get; set; }
        public string RefData { get; set; }
        public string Note { get; set; }
        public string AdminNote { get; set; }

        public int Number { get; set; }
    }
}
