using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;
using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace Website.ViewModels
{
    public class CreateCampaignPaymentViewModel
    {

        public int CampaignId { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

    }

    


}
