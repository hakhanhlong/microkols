using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.ViewModels
{


    
    public class BankViewModel
    {


        public BankViewModel() { }

        public BankViewModel(Bank q) {
            Id = q.Id;
            VietName = q.VietName;
            EngName = q.EngName;
            TradingName = q.TradingName;


        }


        public int Id { get; set; }

        public string VietName { get; set; }
        public string EngName { get; set; }
        public string TradingName { get; set; }


    }

    public class ListBankViewModel
    {
        public List<BankViewModel> Banks { get; set; }
        public PagerViewModel Pager { get; set; }
    }


}
