using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebLandingPage.Models;
using WebServices.Interfaces;

namespace WebLandingPage.Controllers
{
    public class HomeController : Controller
    {

        private readonly IQnAService _IQnAService;
        private readonly IVideoGalleryService _IVideoGalleryService;
        public HomeController(IQnAService __IQnAService, IVideoGalleryService __IVideoGalleryService) {
            _IQnAService = __IQnAService;
            _IVideoGalleryService = __IVideoGalleryService;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [Route("agency.html")]
        public async Task<IActionResult> Agency()
        {
            ViewBag.QnAGeneral = (await _IQnAService.GetByType(Core.Entities.QnAType.General, true, 1)).List_QnA;
            ViewBag.QnAInfluencer = (await _IQnAService.GetByType(Core.Entities.QnAType.Influencer, true, 1)).List_QnA;
            ViewBag.QnAMerchant = (await _IQnAService.GetByType(Core.Entities.QnAType.Merchant, true, 1)).List_QnA;

            return View();
        }

        [Route("influencer.html")]
        public async Task<IActionResult> Influencer()
        {

            ViewBag.QnAGeneral = (await _IQnAService.GetByType(Core.Entities.QnAType.General, true, 1)).List_QnA;
            ViewBag.QnAInfluencer = (await _IQnAService.GetByType(Core.Entities.QnAType.Influencer, true, 1)).List_QnA;
            ViewBag.QnAMerchant = (await _IQnAService.GetByType(Core.Entities.QnAType.Merchant, true, 1)).List_QnA;

            return View();
        }


        [Route("introduce.html")]
        public IActionResult Introduce()
        {
            return View();
        }



        [Route("policy.html")]
        public IActionResult Policy()
        {
            return View();
        }

        [Route("contact.html")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("qna.html")]
        public async Task<IActionResult> QnA()
        {
            ViewBag.QnAGeneral = (await _IQnAService.GetByType(Core.Entities.QnAType.General, true, 1)).List_QnA;
            ViewBag.QnAInfluencer = (await _IQnAService.GetByType(Core.Entities.QnAType.Influencer, true, 1)).List_QnA;
            ViewBag.QnAMerchant = (await _IQnAService.GetByType(Core.Entities.QnAType.Merchant, true, 1)).List_QnA;

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
