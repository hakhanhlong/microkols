using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Bank: BaseEntity
    {
        public string VietName { get; set; }
        public string EngName { get; set; }
        public string TradingName { get; set; }
    }
}
