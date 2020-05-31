using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using WebServices.ViewModels;

namespace BackOffice.Controllers
{
    public class LandingPageController : Controller
    {

        public LandingPageController()
        {

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

        public IActionResult UpdateQnA()
        {
            return View();
        }



        #endregion


    }


}