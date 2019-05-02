using System;

namespace Website.ViewModels
{
    public class PagerViewModel
    {
        public PagerViewModel()
        {

        }

        public PagerViewModel(int page, int pagesize,int total)
        {
            Page = page;
            PageSize = pagesize;
            Total = total;

        }
        public long Total { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)Total / PageSize); }
        }
    }
}