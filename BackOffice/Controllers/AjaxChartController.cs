using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebServices.Interfaces;
using Common.Extensions;
using BackOffice.CommonHelpers;
using BackOffice.Models.Statistic;

namespace BackOffice.Controllers
{




    public class AjaxChartController : Controller
    {

        ITransactionService _ITransactionService;
        public AjaxChartController(ITransactionService __ITransactionService)
        {
            _ITransactionService = __ITransactionService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> Statistic_JsonCampaignPaid([FromBody] DateRangeModel model)
        {

            //format datetime yyyy/MM/dd

            List<string> range_date = new List<string>();
            var endDateTime = DateTime.Now;
            DateTime _startDate = new DateTime(endDateTime.Year, endDateTime.Month, 1);
            if(!string.IsNullOrEmpty(model.startDate) && !string.IsNullOrEmpty(model.endDate))
            {
                _startDate = Convert.ToDateTime(model.startDate);
                endDateTime = Convert.ToDateTime(model.endDate);
            }

            string startDate = _startDate.ToString("MM/dd/yyyy");
            string endDate = endDateTime.ToString("MM/dd/yyyy");


            var result_campaignservice_paid = await _ITransactionService.Statistic_CampaignServicePaid(startDate, endDate, Core.Entities.TransactionStatus.Completed); //line CampaignServicePaid
            var result_campaignaccountpayback_paid = await _ITransactionService.Statistic_CampaignAccountPaybackPaid(startDate, endDate, Core.Entities.TransactionStatus.Completed); //line CampaignAccountPayback
            // create range of date #######################################################################################
            foreach (DateTime day in DateTimeHelpers.EachCalendarDay(_startDate, endDateTime))
            {
                range_date.Add(day.ToString("MM/dd/yyyy"));
            }
            //#############################################################################################################

            var chartData = new object[range_date.Count + 1];
            chartData[0] = new object[]{
                "Ngày tháng",
                "Tiền chiến dịch",
                "Tiền trả Influencer"                
            };

            int j = 0;
            foreach (var str_date in range_date)
            {
                j++;
                long campaignservice_amount = result_campaignservice_paid.Where(r=>r.Timeline == str_date).Select(t=>t.Amount).FirstOrDefault();
                long campaignaccountpayback_amount = result_campaignaccountpayback_paid.Where(r => r.Timeline == str_date).Select(t => t.Amount).FirstOrDefault();
                chartData[j] = new object[] { str_date, campaignservice_amount, campaignaccountpayback_amount};
            }

            
            return Json(chartData);

        }



    }
}