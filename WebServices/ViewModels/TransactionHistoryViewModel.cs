using Common.Extensions;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.ViewModels
{
    public class ListTransactionHistoryViewModel
    {
        public List<TransactionHistoryViewModel> TransactionHistories { get; set; }
        public PagerViewModel Pager { get; set; }
    }
    public class TransactionHistoryViewModel
    {
        public TransactionHistoryViewModel()
        {

        }
        public TransactionHistoryViewModel(TransactionHistory transaction)
        {
            Amount = transaction.Amount;
            Balance = transaction.Balance;
            Note = transaction.Transaction.Note;
            Type = transaction.Transaction.Type;
            RefId = transaction.Transaction.RefId ?? 0;
            Code = transaction.Transaction.Code;
            Status = transaction.Transaction.Status;
            DateCreated = transaction.Transaction.DateCreated;
        }
        public static List<TransactionHistoryViewModel> GetList(IEnumerable<TransactionHistory> transactions)
        {
            return transactions.Select(m => new TransactionHistoryViewModel(m)).ToList();
        }
        public TransactionStatus Status { get; set; }

        public long Amount { get; set; }
        public long Balance { get; set; }
        public string Note { get; set; }
        public int RefId { get; set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; set; }
        public TransactionType Type { get; set; }
    }
}
