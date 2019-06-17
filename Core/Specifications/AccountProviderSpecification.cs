using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{

    public class AccountProviderSpecification : BaseSpecification<AccountProvider>
    {
        public AccountProviderSpecification(int id) : base(m => m.Id == id)
        {

        }
        public AccountProviderSpecification(int accountid, AccountProviderNames provider) : base(m => m.AccountId == accountid && m.Provider == provider)
        {

        }

        public AccountProviderSpecification(AccountProviderNames provider, string providerid)
            : base(m => m.ProviderId == providerid && m.Provider == provider)
        {

        }




    }

    public class AccountProviderByExpiredTokenSpecification : BaseSpecification<AccountProvider>
    {
        
        public AccountProviderByExpiredTokenSpecification(AccountProviderNames provider) 
            : base(m => m.Provider == provider && m.Expired < DateTime.Now.AddHours(2))
        {

        }





    }


}
