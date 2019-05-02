using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification(int id)
         : base(i => i.Id == id)
        {
        }

    }


    public class CategoryPublishedSpecification : BaseSpecification<Category>
    {
        public CategoryPublishedSpecification(int id)
         : base(i => i.Id == id && i.Published)
        {
        }

        public CategoryPublishedSpecification()
            : base(i => i.Published)
        {
        }
    }

}
