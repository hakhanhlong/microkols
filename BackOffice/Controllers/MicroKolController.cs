using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    public class MicroKolController : Controller
    {

        IAccountBusiness _IAccountBusiness;
        IAccountRepository _IAccountRepository;
        public MicroKolController(IAccountBusiness __IAccountBusiness, IAccountRepository __IAccountRepository)
        {
            _IAccountBusiness = __IAccountBusiness;
            _IAccountRepository = __IAccountRepository;
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

        [HttpPost]
        public async Task<IActionResult> Detail(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Id > 0)
                {
                    var microkol = _IAccountRepository.GetById(model.Id);
                    if (microkol == null)
                    {
                        TempData["MessageError"] = "MicroKol do not exist!";
                    }
                    else
                    {
                        microkol.Name = model.Name;
                        microkol.Email = model.Email;
                        microkol.Address = model.Address;
                        microkol.Phone = model.Phone;

                        microkol.Actived = model.Actived;
                        microkol.Deleted = model.Deleted;

                        microkol.IDCardName = model.IDCardName;
                        microkol.IDCardNumber = model.IDCardNumber;
                        microkol.IDCardTime = model.IDCardTime;
                        microkol.IDCardCity = model.IDCardCity;

                        microkol.BankAccountName = model.BankAccountName;
                        microkol.BankAccountNumber = model.BankAccountNumber;
                        microkol.BankAccountBank = model.BankAccountBank;
                        microkol.BankAccountBranch = model.BankAccountBranch;


                        await _IAccountRepository.UpdateAsync(microkol);

                        TempData["MessageSuccess"] = "MicroKol update success!";


                    }
                }
            }
        

            return View(model);
        }


        public async Task<IActionResult> ChangePassword(int id = 0)
        {
            var microkol = await _IAccountBusiness.GetAccount(id);
            if (microkol == null)
            {
                TempData["MessageError"] = "MicroKol do not exist!";
            }

            return View(microkol);
        }


        public async Task<IActionResult> ChangeType(int id = 0)
        {
            var microkol = await _IAccountBusiness.GetAccount(id);

            ViewBag.MocrokolTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Regular", Value = "0"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotTeen", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotMom", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotFacebooker", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Kols", Value = "4"}
            };


            if (microkol == null)
            {
                TempData["MessageError"] = "MicroKol do not exist!";
            }

            return View(microkol);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeType(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    var microkol = _IAccountRepository.GetById(model.Id);
                    if (microkol == null)
                    {
                        TempData["MessageError"] = "MicroKol do not exist!";
                    }
                    else
                    {

                        microkol.Type = model.Type;
                        await _IAccountRepository.UpdateAsync(microkol);
                        TempData["MessageSuccess"] = "MicroKol update microkol type success!";

                    }
                }
            }


            return RedirectToAction("changetype", "microkol", new { id = model.Id });
        }


        public async Task<IActionResult> CampaignCharge(int id = 0)
        {
            var microkol = await _IAccountBusiness.GetAccount(id);

            ViewBag.MocrokolTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Regular", Value = "0"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotTeen", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotMom", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotFacebooker", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Kols", Value = "4"}
            };


            if (microkol == null)
            {
                TempData["MessageError"] = "MicroKol do not exist!";
            }

            return View(microkol);
        }

    }
}