using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    public class CategoryController : Controller
    {

        public CategoryController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}