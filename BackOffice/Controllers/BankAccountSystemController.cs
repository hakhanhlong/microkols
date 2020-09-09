using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebServices.ViewModels;

namespace BackOffice.Controllers
{
    public class BankAccountSystemController : Controller
    {

        private readonly IBankAccountSystemRepository _IBankAccountSystemRepository;

        public BankAccountSystemController(IBankAccountSystemRepository __IBankAccountSystemRepository) {
            _IBankAccountSystemRepository = __IBankAccountSystemRepository;
        }

        public IActionResult Index()
        {

            return View();
        }



        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(EditBankAccountSystemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _IBankAccountSystemRepository.Add(new Core.Entities.BankAccountSystem()
                {
                    BankAccountName = model.BankAccountName,
                    BankAccountNumber = model.BankAccountNumber,
                    BankBranch = model.BankBranch,
                    BankName = model.BankName,
                    IsActive = model.IsActive
                });

                if (result.Id > 0)
                {
                    return RedirectToAction("Index", "BankAccountSystem");
                }
            }

          

            return View();
        }







    }
}