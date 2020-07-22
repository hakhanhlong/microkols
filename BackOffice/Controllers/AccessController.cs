using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    [DisplayName("Access Management")]
    public class AccessController : Controller
    {
        [DisplayName("Access List")]
        public IActionResult Index()
        {
            return View();
        }
    }
}