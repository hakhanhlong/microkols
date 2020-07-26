using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebServices.Code.Helpers;
using WebServices.Interfaces;
using WebServices.Jobs;
using WebServices.ViewModels;
namespace WebMerchant.Controllers
{
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
            return RedirectToAction("ChangeInfo");
        }

        #region UpdateAgency
        public async Task<IActionResult> ChangeInfo()
        {
            var model = await _agencyService.GetUpdateAgency(CurrentUser.Id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeInfo(UpdateAgencyViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Image = _fileHelper.MoveTempFile(model.Image, "agency");
                var r = await _agencyService.UpdateAgency(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                return RedirectToAction("ChangeInfo");
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
                return RedirectToAction("ChangePassword");

            }
            return View(model);
        }
        #endregion
    }
}