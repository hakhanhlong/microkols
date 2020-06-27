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

        public AccountSpecification(string keyword, AccountType type) :
            base(m => (string.IsNullOrEmpty(keyword) || m.Email.Contains(keyword) || m.Name.Contains(keyword) || m.Phone.Contains(keyword) || m.IDCardNumber.Contains(keyword)) 
            && ( type== AccountType.All ||m.Type == type))
        {

        }


        public AccountSpecification(string email, string name) : base(m => m.Email.Contains(email) || m.Name.Contains(name) || m.Phone.Contains(name) || m.IDCardNumber.Contains(name))
        {

        }

    }

    public class AccountWithCategorySpecification : BaseSpecification<Account>
    {
        public AccountWithCategorySpecification(int accountid) : base(m => m.Id == accountid)
        {
            AddInclude(m => m.AccountCategory);
        }
    }



}
