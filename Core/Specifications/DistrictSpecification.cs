using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{

    public class DistrictSpecification : BaseSpecification<District>
    {
       
        public DistrictSpecification(int cityid)
           : base(i => i.CityId == cityid)
        {

        }


      



    }

}
