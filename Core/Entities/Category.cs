using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Category : BaseEntityWithMeta
    {

        public Category()
        {            
        }

        public string Name { get; set; }
        
        public ICollection<AccountCategory> AccountCategory { get; set; }
    }
}
