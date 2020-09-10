using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServices.ViewModels;

namespace BackOffice.Controllers
{
    [Authorize]
    public class BankAccountSystemController : Controller
    {

        private readonly IBankAccountSystemRepository _IBankAccountSystemRepository;

        public BankAccountSystemController(IBankAccountSystemRepository __IBankAccountSystemRepository) {
            _IBankAccountSystemRepository = __IBankAccountSystemRepository;
        }

        public IActionResult Index()
        {
            var listing = _IBankAccountSystemRepository.ListAll();

            return View(listing);
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


        public IActionResult Edit(int id)
        {
            var bank = _IBankAccountSystemRepository.GetById(id);
            return View(new EditBankAccountSystemViewModel(bank));
        }


        [HttpPost]
        public IActionResult Edit(EditBankAccountSystemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bank = _IBankAccountSystemRepository.GetById(model.Id);

                bank.BankAccountName = model.BankAccountName;
                bank.BankAccountNumber = model.BankAccountNumber;
                bank.BankBranch = model.BankBranch;
                bank.BankName = model.BankName;
                bank.IsActive = model.IsActive;

                _IBankAccountSystemRepository.Update(bank);

                TempData["MessageSuccess"] = "Cập nhật tài khoản ngân hàng thành công!";

                return RedirectToAction("Index", "BankAccountSystem");
            }
            return View();
        }


    }
}