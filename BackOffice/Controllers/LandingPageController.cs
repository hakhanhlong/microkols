using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.CommonHelpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebServices.Interfaces;
using WebServices.ViewModels;

namespace BackOffice.Controllers
{
    public class LandingPageController : Controller
    {

        private readonly IQnARepository _IQnARepository;
        private readonly IQnAService _IQnAService;

        private readonly IVideoGalleryRepository _IVideoGalleryRepository;
        private readonly IVideoGalleryService _IVideoGalleryService;

        public LandingPageController(IQnARepository __IQnARepository, IQnAService __IQnAService, IVideoGalleryRepository __IVideoGalleryRepository,
            IVideoGalleryService __IVideoGalleryService)
        {
            _IQnARepository = __IQnARepository;
            _IQnAService = __IQnAService;
            _IVideoGalleryRepository = __IVideoGalleryRepository;
            _IVideoGalleryService = __IVideoGalleryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region QnA
        public async Task<IActionResult> QnA(QnAType? type, int pageindex = 1)
        {
            var list = await _IQnAService.GetByType(type, true, pageindex);
            return View(list);
        }

        public IActionResult CreateQnA()
        {
            return View("UpdateQnA", new QnAViewModel() { Id = 0 });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateQnA(int Id = 0)
        {
            if (Id != 0)
            {
                var qna = await _IQnARepository.GetByIdAsync(Id);
                if (qna != null)
                {
                    return View(new QnAViewModel()
                    {
                        Id = qna.Id,
                        Question = qna.Question,
                        Answer = qna.Answer,
                        IsActive = qna.IsActive,
                        Order = qna.Order
                    });

                }
                else
                {
                    TempData["MessageError"] = string.Format("{0}", "QnA empty, do not exist!");
                }

            }

            return View(new QnAViewModel() { Id = 0 });

        }

        [HttpPost]
        public async Task<IActionResult> UpdateQnA(QnAViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                if (model.Id > 0) //edit
                {
                    model.UserModified = HttpContext.User.Identity.Name;
                    var retValue = await _IQnAService.Update(model);
                    if(retValue > 0) {
                        TempData["MessageSuccess"] = string.Format("{0}", "Thay đổi QnA thành công");
                    }
                    else {
                        TempData["MessageError"] = string.Format("{0}", "Thay đổi QnA không thành công");
                    }
                }
                else //insert
                {
                    model.UserCreated = model.UserModified = HttpContext.User.Identity.Name;
                    var retValue = await _IQnAService.Create(model);
                    if (retValue > 0)
                    {
                        TempData["MessageSuccess"] = string.Format("{0}", "Thêm mới QnA thành công");
                    }
                    else
                    {
                        TempData["MessageError"] = string.Format("{0}", "Thêm mới QnA không thành công");
                    }
                }
            }
            else
            {
                TempData["MessageError"] = string.Format("{0}", "ModelState Invalid");
            }
            return View(model);
        }

        #endregion

        #region Video Gallery
        public async Task<IActionResult> VideoGallery(int pageindex = 1)
        {
            var list = await _IVideoGalleryService.GetAll(pageindex);
            return View(list);
        }


        public IActionResult CreateVideoGallery()
        {
            return View("UpdateVideoGallery", new VideoGalleryViewModel() { Id = 0 });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateVideoGallery(int Id = 0)
        {
            if (Id != 0)
            {
                var videoGallery = await _IVideoGalleryRepository.GetByIdAsync(Id);
                if (videoGallery != null)
                {
                    return View(new VideoGalleryViewModel()
                    {
                        Id = videoGallery.Id,                        
                        IsActive = videoGallery.IsActive,
                        Order = videoGallery.Order,
                        ImageURL = videoGallery.ImageURL,
                        VideoEmbed = videoGallery.VideoEmbed,
                        EmbedKey = videoGallery.EmbedKey
                    });

                }
                else
                {
                    TempData["MessageError"] = string.Format("{0}", "Video Gallery empty, do not exist!");
                }

            }

            return View(new VideoGalleryViewModel() { Id = 0 });

        }



        [HttpPost]
        public async Task<IActionResult> UpdateVideoGallery(VideoGalleryViewModel model)
        {
            if (model.Id > 0) //edit
            {
                model.UserModified = HttpContext.User.Identity.Name;
                if(model.fileUpload != null)
                {
                    string filePath = await FileHelpers.UploadFile(model.fileUpload, "videogallery");
                    model.ImageURL = filePath;
                }
                

                var retValue = await _IVideoGalleryService.Update(model);
                if (retValue > 0)
                {
                    TempData["MessageSuccess"] = string.Format("{0}", "Thay đổi Video Gallery thành công");
                }
                else
                {
                    TempData["MessageError"] = string.Format("{0}", "Thay đổi Video Gallery không thành công");
                }


            }
            else //insert
            {
                model.UserCreated = model.UserModified = HttpContext.User.Identity.Name;
                if (model.fileUpload != null)
                {
                    string filePath = await FileHelpers.UploadFile(model.fileUpload, "videogallery");
                    model.ImageURL = filePath;
                }

                var retValue = await _IVideoGalleryService.Create(model);
                if (retValue > 0)
                {
                    TempData["MessageSuccess"] = string.Format("{0}", "Thêm mới Video Gallery thành công");
                }
                else
                {
                    TempData["MessageError"] = string.Format("{0}", "Thêm mới Video Gallery không thành công");
                }


            }

            return View(model);
        }


        #endregion


    }


}