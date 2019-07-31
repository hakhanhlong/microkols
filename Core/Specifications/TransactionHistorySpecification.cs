﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class TransactionHistorySpecification : BaseSpecification<TransactionHistory>
    {
        public TransactionHistorySpecification(int walletid, Common.Helpers.DateRange? dateRange) : base(m => m.WalletId == walletid &&
        (!dateRange.HasValue || (m.DateCreated> dateRange.Value.Start && m.DateCreated > dateRange.Value.End))        
        )
        {
            AddInclude(m => m.Transaction);
        }

    }

   

}