
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Entities
{
    public class Audit : BaseEntity
    {
        public string TableName { get; set; }
        public int TableId { get; set; }
        public DateTime DateTime { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
    }
  
}
