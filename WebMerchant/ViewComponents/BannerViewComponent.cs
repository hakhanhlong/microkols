using WebServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMerchant.ViewComponents
{
    public class BannerViewComponent : ViewComponent
    {
        private readonly ISharedService _sharedService;
        public BannerViewComponent(ISharedService sharedService)
        {
            _sharedService = sharedService;
        }
        public async Task<IViewComponentResult> InvokeAsync(Core.Entities.BannerPosition position, string vname = "Default")
        {
            var model = await _sharedService.GetBanners(position);
            return View(vname, model);
        }

    }
}
