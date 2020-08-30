using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common;
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




            ViewBag.VideoGalleries = (await _IVideoGalleryService.GetByType(true, 1)).VideoGalleries;

            return View();
        }

        [Route("influencer.html")]
        public async Task<IActionResult> Influencer()
        {

            ViewBag.QnAGeneral = (await _IQnAService.GetByType(Core.Entities.QnAType.General, true, 1)).List_QnA;
            ViewBag.QnAInfluencer = (await _IQnAService.GetByType(Core.Entities.QnAType.Influencer, true, 1)).List_QnA;
            ViewBag.QnAMerchant = (await _IQnAService.GetByType(Core.Entities.QnAType.Merchant, true, 1)).List_QnA;

            ViewBag.VideoGalleries = (await _IVideoGalleryService.GetByType(true, 1)).VideoGalleries;

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


        [Route("contact.html")]
        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            try
            {
                                
                string from = model.Email;
                string to = "contact.microkols@gmail.com";
                string subject = $"[Liên hệ] từ {model.HovaTen}";

                string plainText = $"Thông tin liên hệ của {model.HovaTen},";

                string htmlText = string.Empty;
                htmlText += $"<p>Họ và tên: {model.HovaTen}</p>";
                if (!string.IsNullOrEmpty(model.Phone))
                {
                    htmlText += $"<p>Số điện thoại: {model.Phone}</p>";
                }
                htmlText += $"<p>Email: {model.Email}</p>";
                htmlText += $"<p>Công ty: {model.Cty}</p>";
                htmlText += $"<p>Nội dung: {model.Noidung}</p>";

                
                await SendEmailHelpers.SendEmailFromContact(from, to, subject, plainText, htmlText, model.HovaTen);
                TempData["MessageInfo"] = "Đã gửi thành công, cám ơn bạn đã liên hệ.";
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.Message;
            }
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
