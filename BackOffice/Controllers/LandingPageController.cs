using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebServices.ViewModels;

namespace BackOffice.Controllers
{
    public class LandingPageController : Controller
    {

        private readonly IQnARepository _IQnARepository;

        public LandingPageController(IQnARepository __IQnARepository)
        {
            _IQnARepository = __IQnARepository;
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



        #endregion


    }


}