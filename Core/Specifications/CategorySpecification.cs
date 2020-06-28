using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification() : base() { }

        public CategorySpecification(int id)
         : base(i => i.Id == id)
        {
            AddInclude(c => c.AccountCategory);
            AddInclude($"{nameof(Category.AccountCategory)}.{nameof(AccountCategory.Account)}");
        }

    }


    public class CategoryPublishedSpecification : BaseSpecification<Category>
    {
        public CategoryPublishedSpecification(int id)
         : base(i => i.Id == id && i.Published)
        {
            AddInclude(c => c.AccountCategory);
            AddInclude($"{nameof(Category.AccountCategory)}.{nameof(AccountCategory.Account)}");
        }

        public CategoryPublishedSpecification()
            : base(i => i.Published)
        {
            AddInclude(c => c.AccountCategory);
            AddInclude($"{nameof(Category.AccountCategory)}.{nameof(AccountCategory.Account)}");
        }
    }

}
