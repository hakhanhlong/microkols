using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class BannerSpecification : BaseSpecification<Banner>
    {
        public BannerSpecification(int id)
         : base(i => i.Id == id)
        {
        }

        public BannerSpecification(string kw,BannerPosition position)
            : base(i =>  i.Position== position&& ((string.IsNullOrEmpty(kw) || i.Title.Contains(kw))))
        {
        }
    }


    public class BannerPublishedSpecification : BaseSpecification<Banner>
    {
        public BannerPublishedSpecification(int id)
         : base(i => i.Id == id && i.Published)
        {
        }

        public BannerPublishedSpecification(BannerPosition position)
           : base(i => i.Published && i.Position == position)
        {
        }

        public BannerPublishedSpecification()
            : base(i => i.Published)
        {
        }
    }

}
