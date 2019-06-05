using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{

    [Authorize]
    public class AgencyController : Controller
    {        
        private readonly IAgencyBusiness _IAgencyBusiness;

        public AgencyController(IAgencyBusiness __IAgencyBusiness)
        {
            _IAgencyBusiness = __IAgencyBusiness;
        }

        public IActionResult Index(int pageindex = 1)
        {
            var list_agency = _IAgencyBusiness.GetListAgency(pageindex, 20);



            return View(list_agency);
        }
    }


}