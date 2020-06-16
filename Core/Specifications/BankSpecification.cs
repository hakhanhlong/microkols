using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class BankSpecification : BaseSpecification<Bank>
    {
        public BankSpecification(int id) : base(m => m.Id > 0)
        {}
    }
}
