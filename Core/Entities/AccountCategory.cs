using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
   public  class AccountCategory
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
