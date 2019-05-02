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

        public AccountProviderSpecification(string provider, string providerid)
            : base(m => m.ProviderId == providerid && m.Provider == provider)
        {

        }




    }

}
