using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class AgencySpecification : BaseSpecification<Agency>
    {
        public AgencySpecification(int id) : base(m => m.Id == id)
        {

        }
        public AgencySpecification(string username) : base(m => m.Username == username)
        {

        }

        public AgencySpecification(string taxnumber, string name) : base(m => m.TaxIdNumber.Contains(taxnumber) || m.Name.Contains(name))
        {

        }



    }

   

}
