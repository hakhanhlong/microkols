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

        private readonly IAgencyService _accountService;
        private readonly ISharedService _sharedService;
        private readonly IFileHelper _fileHelper;
        public AgencyController(IAgencyService accountService, ISharedService sharedService, IFileHelper fileHelper)
        {

            _accountService = accountService;
            _sharedService = sharedService;
            _fileHelper = fileHelper;


        }



        #region UpdateAgency
        public async Task<IActionResult> UpdateAgency()
        {
            var model = await _accountService.GetUpdateAgency(CurrentUser.Id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAgency(UpdateAgencyViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Image = _fileHelper.MoveTempFile(model.Image, "agency");
                var r = await _accountService.UpdateAgency(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                return RedirectToAction("UpdateAgency");
            }
            return View(model);
        }
        #endregion

    }
}