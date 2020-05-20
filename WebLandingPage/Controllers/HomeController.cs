using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebLandingPage.Models;

namespace WebLandingPage.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [Route("agency.html")]
        public IActionResult Agency()
        {
            return View();
        }

        [Route("influencer.html")]
        public IActionResult Influencer()
        {
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
        public IActionResult QnA()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
