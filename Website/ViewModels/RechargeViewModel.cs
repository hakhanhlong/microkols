
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
    public class RechargeViewModel
    {
        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Range(1000, 100000000000, ErrorMessage = "{0} không đúng")]
        [Display(Name = "Số tiền")]
        public long Amount { get; set; }

        [Display(Name = "Hình thức")]
        public string Method { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        [Display(Name = "Ngân hàng")]
        public string Bank { get; set; }

        public int CampaignId { get; set; }


        public string GetTransactionData()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new TransactionDataRechargeModel()
            {
                Bank = Bank,
                Method = Method,
            });
        }
    }
}
