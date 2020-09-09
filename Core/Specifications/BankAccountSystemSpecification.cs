using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class BankAccountSystemSpecification : BaseSpecification<BankAccountSystem>
    {
        public BankAccountSystemSpecification(int id) : base(m => m.Id > 0)
        {}
    }
}
