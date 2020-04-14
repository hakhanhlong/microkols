using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class TransactionHistorySpecification : BaseSpecification<TransactionHistory>
    {
        public TransactionHistorySpecification(int walletid, Common.Helpers.DateRange? dateRange) : base(m => m.WalletId == walletid &&
        (!dateRange.HasValue || (m.DateCreated.Date >= dateRange.Value.Start.Date && m.DateCreated.Date <= dateRange.Value.End.Date))        
        )
        {
            AddInclude(m => m.Transaction);
        }

        public TransactionHistorySpecification(int walletid, Common.Helpers.DateRange? dateRange,TransactionType? type) : base(m => m.WalletId == walletid &&
      (!dateRange.HasValue || (m.DateCreated.Date >= dateRange.Value.Start.Date && m.DateCreated.Date <= dateRange.Value.End.Date)) &&
          (!type.HasValue || (m.Transaction.Type== type))
      )
        {
            AddInclude(m => m.Transaction);
        }


        public TransactionHistorySpecification(int walletid) : base(m => m.WalletId == walletid)
        {
            AddInclude(m => m.Transaction);
            AddInclude(m => m.Wallet);
        }

    }

    public class TransactionHistoryWithTransactionSpecification : BaseSpecification<TransactionHistory>
    {
        


        public TransactionHistoryWithTransactionSpecification(int transactionid) : base(m => m.TransactionId == transactionid)
        {
            AddInclude(m => m.Transaction);
            AddInclude(m => m.Wallet);
        }

    }



}
