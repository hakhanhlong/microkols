using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServices.ViewModels
{
    public class TransactionStatisticViewModel
    {


        public TransactionStatisticViewModel(TransactionStatistic entity)
        {
            Timeline = entity.Timeline;
            Type = entity.Type;
            Amount = entity.Amount;
        }

        public string Timeline { get; set; }
        public TransactionType Type { get; set; }
        public long Amount { get; set; }
    }
}
