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
    [Authorize(Roles = "Agency")]
    public class AgencyController : BaseController
    {

        private readonly IAgencyService _agencyService;
        private readonly ISharedService _sharedService;
        private readonly IFileHelper _fileHelper;
        public AgencyController(IAgencyService agencyService, ISharedService sharedService, IFileHelper fileHelper)
        {

            _agencyService = agencyService;
            _sharedService = sharedService;
            _fileHelper = fileHelper;


        }

        public async Task<IActionResult> Index()
        {

            return View();
        }


        #region UpdateAgency
        public async Task<IActionResult> UpdateAgency()
        {
            var model = await _agencyService.GetUpdateAgency(CurrentUser.Id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAgency(UpdateAgencyViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Image = _fileHelper.MoveTempFile(model.Image, "agency");
                var r = await _agencyService.UpdateAgency(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                return RedirectToAction("UpdateAgency");
            }
            return View(model);
        }
        #endregion



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
                var r = await _agencyService.ChangePassword(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                return RedirectToAction("ChangeIDCard");

            }
            return View(model);
        }
        #endregion

    }
}