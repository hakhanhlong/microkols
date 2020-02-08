using WebServices.ViewModels;
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

namespace WebMerchant.Code.TagHelpers
{
    [HtmlTargetElement("btnsharefacebook")]

    public class ButtonShareFacebookTagHelper : TagHelper
    {
        public string Url { get; set; }
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var url = WebUtility.UrlEncode(Url);
            output.TagName = "span";
            output.Attributes.Add("class", $"btn btn-facebook btn-share-social");
            output.Attributes.Add("data-url",$"https://www.facebook.com/sharer/sharer.php?u={url}");

            output.Content.SetHtmlContent($"<i class='fab fa-facebook-f'></i>");
        }
    }

}
