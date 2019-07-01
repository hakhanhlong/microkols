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

        }


        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        public string Code { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public long Amount { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public string Data { get; set; }
        public int? RefId { get; set; }
        public string RefData { get; set; }
        public string Note { get; set; }
        public string AdminNote { get; set; }
    }
}
