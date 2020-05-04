using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Extensions;

namespace WebServices.ViewModels
{
    public class TransactionViewModel
    {
        public TransactionViewModel()
        {

        }
        public TransactionViewModel(Transaction  transaction)
        {
            Code = transaction.Code;
            SenderId = transaction.SenderId;
            ReceiverId = transaction.ReceiverId;
            Amount = transaction.Amount;
            Type = transaction.Type;
            Status = transaction.Status;
            Data = transaction.Data;
            RefId = transaction.RefId;
            Note = transaction.Note;

            AmountToText = Amount.ToPriceText();

            StatusToText = Status.ToString();

            CreatedDate = transaction.DateCreated;
        }
        public static List<TransactionViewModel> GetList(IEnumerable<Transaction> transactions)
        {
            return transactions.Select(m => new TransactionViewModel(m)).ToList();
        }

        public string Code { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public long Amount { get; set; }

        public string AmountToText { get; set; }

        public string StatusToText { get; set; }

        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public string Data { get; set; }
        public int? RefId { get; set; }
        public string Note { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
