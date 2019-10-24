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
        public AccountFbPostSpecification(int accountid,string postid) : base(m => m.PostId == postid && m.AccountId == accountid)
        {

        }

    }

    public class AccountFbPostByAccountSpecification : BaseSpecification<AccountFbPost>
    {
        public AccountFbPostByAccountSpecification(int accountid) : base(m => m.AccountId == accountid)
        {

        }

        public AccountFbPostByAccountSpecification(List<string> refid) : base(m => refid.Contains(m.PostId))
        {

        }
    }

}
