using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public LandingPageController(IQnARepository __IQnARepository, IQnAService __IQnAService)
        {
            _IQnARepository = __IQnARepository;
            _IQnAService = __IQnAService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region QnA
        public IActionResult QnA(QnAType? type, int pageindex = 1)
        {
            return View();
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


    }


}