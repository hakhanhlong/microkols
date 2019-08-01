﻿using Core.Entities;
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

        public AccountSpecification(string keyword, AccountType type) : base(m => (m.Email.Contains(keyword) || m.Name.Contains(keyword)) && m.Type == type)
        {

        }


        public AccountSpecification(string email, string name) : base(m => m.Email.Contains(email) || m.Name.Contains(name))
        {

        }

    }

   

}
