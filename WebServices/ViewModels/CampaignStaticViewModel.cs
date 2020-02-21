using Common.Extensions;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.ViewModels
{
    public class CampaignAccountStatisticViewModel
    {
        public CampaignAccountStatisticViewModel()
        {

        }
        public static List<CampaignAccountStatisticViewModel> GetList(IEnumerable<CampaignAccountStatistic> statistics)
        {
            var result = new List<CampaignAccountStatisticViewModel>();

            result.Add(new CampaignAccountStatisticViewModel()
            {
                Date = "18-2-2020",
                CountComment = 23,
                CountLike = 41,
                CountShare = 54
            });
            result.Add(new CampaignAccountStatisticViewModel()
            {
                Date = "19-2-2020",
                CountComment = 43,
                CountLike = 111,
               CountShare = 142
            });
            result.Add(new CampaignAccountStatisticViewModel()
            {
                Date = "20-2-2020",
                CountComment = 223,
                CountLike = 241,
                CountShare = 454
            });
            result.Add(new CampaignAccountStatisticViewModel()
            {
                Date = "21-2-2020",
                CountComment = 423,
                CountLike = 451,
                CountShare = 524
            });



            //var dateRanges = statistics.Select(m => m.Date).Distinct().OrderBy(m=>m);
            //foreach(var item in dateRanges)
            //{

            //    var query = statistics.Where(m => m.Date == item);
            //    result.Add(new CampaignAccountStatisticViewModel()
            //    {
            //        Date = item.ToViDate(),
            //        CountComment = query.Sum(m=>m.CountComment),
            //        CountLike = query.Sum(m => m.CountLike),
            //        CountShare = query.Sum(m => m.CountShare),
            //    });
            //}

            return result;

        }
        public string Date { get; set; }
        public int CountLike { get; set; }
        public int CountShare { get; set; }
        public int CountComment { get; set; }
    }
}
