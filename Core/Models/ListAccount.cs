using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class ListAccount
    {
        public List<Account> Accounts { get; set; }
        public int Total { get; set; }
    }
}
