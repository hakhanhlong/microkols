using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{

    public class AccountFbPostSpecification : BaseSpecification<AccountFbPost>
    {
        public AccountFbPostSpecification(string postid) : base(m => m.PostId == postid)
        {

        }


    }

    public class AccountFbPostByAccountSpecification : BaseSpecification<AccountFbPost>
    {
        public AccountFbPostByAccountSpecification(int accountid) : base(m => m.AccountId == accountid)
        {

        }


    }

}
