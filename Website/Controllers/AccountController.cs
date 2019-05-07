using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Code.Helpers;
using Website.Interfaces;
using Website.ViewModels;

namespace Website.Controllers
{
    [Authorize(Roles ="Account")]
    public class AccountController : BaseController
    {

        private readonly IAccountService _accountService;
        private readonly ISharedService _sharedService;
        private readonly IFileHelper _fileHelper;
        public AccountController(IAccountService accountService, ISharedService sharedService, IFileHelper fileHelper)
        {

            _accountService = accountService;
            _sharedService = sharedService;
            _fileHelper = fileHelper;


        }

        #region ChangePassword
        public async Task<IActionResult> ChangePassword()
        {
            var model = new ChangePasswordViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _accountService.ChangePassword(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                return RedirectToAction("ChangeIDCard");

            }
            return View(model);
        }
        #endregion

        #region Change Info
        public async Task<IActionResult> ChangeIDCard()
        {
            var model = await _accountService.GetIDCard(CurrentUser.Id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeIDCard(ChangeIDCardViewModel model)
        {
            if (ModelState.IsValid)
            {
                //--> move temp folder -> resources

                model.ImageBack = _fileHelper.MoveTempFile(model.ImageBack,"account");
                model.ImageFront = _fileHelper.MoveTempFile(model.ImageFront, "account");

                var r = await _accountService.ChangeIDCard(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                return RedirectToAction("ChangeIDCard");

            }
            return View(model);
        }
        #endregion
        #region Change Info
        public async Task<IActionResult> ChangeInfo()
        {
            var model = await _accountService.GetInformation(CurrentUser.Id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeInfo(ChangeInformationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _accountService.ChangeInformation(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                return RedirectToAction("ChangeInfo");

            }
            return View(model);
        }
        #endregion

        #region ChangeBankAccount
        public async Task<IActionResult> ChangeBankAccount()
        {
            var model = await _accountService.GetBankAccount(CurrentUser.Id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeBankAccount(ChangeBankAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _accountService.ChangeBankAccount(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                return RedirectToAction("ChangeBankAccount");

            }
            return View(model);
        }
        #endregion


        #region Change Contact
        private async Task ViewbagAddress()
        {
            var cities = await _sharedService.GetCities();
            ViewBag.Cities = cities;
            var city = cities.FirstOrDefault();
            var districtid = city.Id;
            ViewBag.Districts = await _sharedService.GetDistricts(districtid);
        }


        public async Task<IActionResult> ChangeContact()
        {
            var model = await _accountService.GetContact(CurrentUser.Id);
            await ViewbagAddress();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeContact(ChangeContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _accountService.ChangeContact(CurrentUser.Id, model, CurrentUser.Username);

                if (r)
                {
                    await ReSignIn(CurrentUser.Id);
                    this.AddAlert(true);
                    return RedirectToAction("ChangeContact");
                }

            }
            await ViewbagAddress();
            return View(model);
        }

        #endregion


        #region Helper
        private async Task ReSignIn(int id)
        {
            var auth = await _accountService.GetAuth(CurrentUser.Id);
            await SignIn(auth);
        }
        #endregion

    }
}