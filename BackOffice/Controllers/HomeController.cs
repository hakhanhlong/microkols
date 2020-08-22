using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using BackOffice.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using BackOffice.CommonHelpers;

namespace BackOffice.Controllers
{


    [Authorize]
    public class HomeController : Controller
    {
        ICampaignBusiness _ICampaignBusiness;

        public HomeController(ICampaignBusiness __ICampaignBusiness) {
            _ICampaignBusiness = __ICampaignBusiness;
        }


        [Authorize]
        public IActionResult Index()
        {
            var listing_campaign = _ICampaignBusiness.GetListCampaign(1, 15);
            //if(listing_campaign != null)
            //{
            //    ViewBag.Listing_Campaign = listing_campaign;
            //}


            return View(listing_campaign);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpPost]
        public async Task<IActionResult> ImageUpload(IFormFile file, string path = "VideoGallery")
        {

            if (file != null)
            {
                var image = await FileHelpers.UploadFile(file, path);

                if (!string.IsNullOrEmpty(image))
                {
                    return Ok(new
                    {
                        status = 1,
                        image = image
                    });
                }
            }

            return Ok(new
            {
                status = 0,
                message = "Không đúng định dạng ảnh",
            });
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] List<IFormFile> files, string path = "")
        {

            var images = new List<object>();


            int count = 1;
            foreach (var item in files)
            {
                if (count <= 6)//giới hạn chỉ up <= 6 ảnh
                {
                    var image = await FileHelpers.UploadFile(item, path);
                    if (!string.IsNullOrEmpty(image))
                    {
                        images.Add(new
                        {
                            path = image,
                            url = image
                        });
                    }
                    count++;
                }

            }
            return Json(images);
        }


       


        [Route("/api/editorimageupload")]
        [HttpPost]
        public async Task<IActionResult> EditorImageUpload(IFormFile file, string path = "Editors")
        {

            if (file != null)
            {
                var image = await FileHelpers.UploadFile(file, path);

                if (!string.IsNullOrEmpty(image))
                {
                    return Ok(new
                    {
                        status = 1,
                        image = image,
                        imageurl = $"{AppConstants.RESOURCE_SERVER}/{image}"
                    });
                }
            }

            return Ok(new
            {
                status = 0,
                message = "Không đúng định dạng ảnh",
            });
        }

      







    }
}
