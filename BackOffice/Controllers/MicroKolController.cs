using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    public class MicroKolController : Controller
    {

        IAccountBusiness _IAccountBusiness;
        public MicroKolController(IAccountBusiness __IAccountBusiness)
        {
            _IAccountBusiness = __IAccountBusiness;
        }

        public IActionResult Index(int pageindex = 1)
        {

            var list = _IAccountBusiness.GetListAccount(pageindex, 25);
            if(list == null)
            {
                TempData["MessageError"] = "No data microkols for binding";
            }
            return View(list);
        }
    }


}