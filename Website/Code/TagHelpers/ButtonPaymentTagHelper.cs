using Website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using System.Net;

namespace Website.Code.TagHelpers
{
    [HtmlTargetElement("btnpayment")]

    public class ButtonPaymentTagHelper : TagHelper
    {
        public int CampaignId { get; set; }
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.Attributes.Add("class", $"btn btn-primary btn-payment");
            output.Attributes.Add("data-id", CampaignId);

            output.Content.SetHtmlContent($"Thanh toán");
        }
    }

}
