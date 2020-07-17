using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebInfluencer.Models;
using WebServices.Code.Helpers;
using WebServices.Interfaces;
using WebServices.Jobs;

namespace WebInfluencer.Controllers
{

    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IFileHelper _fileHelper;
        private readonly ISharedService _sharedService;

        private readonly IAccountService _accountService;
        IFacebookJob _IFacebookJob;

        public HomeController(IHostingEnvironment hostingEnvironment, IFileHelper fileHelper, ISharedService sharedService, 
            IAccountService __IAccountService, IFacebookJob __IFacebookJob)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileHelper = fileHelper;
            _sharedService = sharedService;
            _accountService = __IAccountService;
            _IFacebookJob = __IFacebookJob;
        }

        public async Task<IActionResult> Index()
        {
            //BackgroundJob.Enqueue<IFacebookJob>(m => m.UpdateFbPost(CurrentUser.Id, CurrentUser.Username, 2));
            //await _IFacebookJob.UpdateFbInfo(CurrentUser.Id);
            return RedirectToAction("MarketPlace","Campaign");
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Api
        [HttpPost]
        public async Task<IActionResult> UploadImage(List<IFormFile> files)
        {
           

            var result = new List<object>();

            foreach (var formFile in files)
            {
                var newpath = await _fileHelper.UploadTempFile(formFile);

                if (!string.IsNullOrEmpty(newpath))
                {
                    result.Add(new
                    {
                        path = newpath,
                        url = _fileHelper.GetImageUrl(newpath)
                    });
                }
            }

            return Json(result);
        }

        public async Task<IActionResult> GetDistricts(int cityid)
        {
            var model = await _sharedService.GetDistricts(cityid);
            return Json(model);
        }

        #endregion

    }
}
