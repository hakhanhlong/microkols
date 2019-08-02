using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using Core.Entities;
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


        public IActionResult Search(string keyword, AccountType type, int pageindex = 1)
        {

            var list = _IAccountBusiness.Search(keyword, type, pageindex, 25);
            if (list == null)
            {
                TempData["MessageError"] = "No data microkols for binding";
            }
            return View(list);
        }

        public async Task<IActionResult> Detail(int id = 0)
        {
            var microkol = await _IAccountBusiness.GetAccount(id);
            if(microkol == null)
            {
                TempData["MessageError"] = "MicroKol do not exist!";
            }

            return View(microkol);
        }


    }


}