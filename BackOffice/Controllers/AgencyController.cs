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

        public IActionResult Search(string keyword, int pageindex = 1)
        {
            var list_agency = _IAgencyBusiness.Search(keyword, pageindex, 25);
            return View(list_agency);
        }

        public IActionResult Active(int id)
        {
            
            if (_IAgencyBusiness.Active(id))
            {
                TempData["MessageSuccess"] = "Active Agency Success!";
            }
            else
            {
                TempData["MessageError"] = "Active Agency Error!";
            }


            return RedirectToAction("index", "agency");
        }


        public IActionResult UnActive(int id)
        {
            
            if (_IAgencyBusiness.UnActive(id))
            {
                TempData["MessageSuccess"] = "UnActive Agency Success!";
            }
            else
            {
                TempData["MessageError"] = "UnActive Agency Error!";
            }

            return RedirectToAction("index", "agency");

        }


    }


}