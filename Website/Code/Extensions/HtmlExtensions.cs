using Common.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Website.Code.Extensions
{
    public static class HtmlExtensions
    {


        public static string AbsoluteAction(this IUrlHelper url, string actionName, string controllerName, object routeValues = null)
        {
            string scheme = url.ActionContext.HttpContext.Request.Scheme;
            return url.Action(actionName, controllerName, routeValues, scheme);
        }

        public static string GetString(this IHtmlContent content)
        {
            //return new HtmlString(content.ToString());
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);

            return writer.ToString();
        }
        public static HtmlString ToBadge(this Core.Entities.CampaignStatus status)
        {
            var type = "success";
            return new HtmlString($"<span class='badge badge-{type}'>{status.ToDisplayName()}</span>");
        }

        public static HtmlString ToIcon(this bool published)
        {
            var icon = "";

            if (published)
            {
                icon = "fa-check text-success";
            }
            else
            {
                icon = "fa-times text-danger";
            }
            return new HtmlString($"<span class='fas {icon}'></span>");
        }
    }


}
