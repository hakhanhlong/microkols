using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class AccountSpecification : BaseSpecification<Account>
    {
        public AccountSpecification(int accountid) : base(m => m.Id == accountid)
        {

        }
        public AccountSpecification(string email) : base(m => m.Email == email)
        {

        }
    }

   

}
