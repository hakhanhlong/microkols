using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    public class CampaignTypeChargeController : Controller
    {

        private readonly ICampaignTypeChargeRepository _ICampaignTypeChargeRepository;

        public CampaignTypeChargeController(ICampaignTypeChargeRepository __ICampaignTypeChargeRepository)
        {
            _ICampaignTypeChargeRepository = __ICampaignTypeChargeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}