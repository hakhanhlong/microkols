using Core.Entities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BackOffice.Extensions
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
            var type = "primary";
            if (status == CampaignStatus.Canceled)
            {
                type = "warning";
            }
            else if (status == CampaignStatus.Error || status == CampaignStatus.Locked)
            {
                type = "danger";
            }
            else if (status == CampaignStatus.Started)
            {
                type = "info";
            }

            else if (status == CampaignStatus.Ended)
            {
                type = "accent";
            }
            else if (status == CampaignStatus.Completed)
            {
                type = "success";
            }
            else if (status == CampaignStatus.Confirmed)
            {
                type = "primary";
            }
            else
            {
                type = "meta";
            }

            return new HtmlString($"<span class='m-badge m-badge--{type} m-badge--wide'>{status.ToDisplayName()}</span>");
        }

        public static HtmlString ToBadge(this Core.Entities.TransactionStatus status)
        {
            var type = "primary";
            if (status == TransactionStatus.Canceled)
            {
                type = "warning";
            }
            else if (status == TransactionStatus.Error)
            {
                type = "danger";
            }
            else if (status == TransactionStatus.Processing)
            {
                type = "info";
            }

            else if (status == TransactionStatus.Created)
            {
                type = "accent";
            }
            else if (status == TransactionStatus.Completed)
            {
                type = "success";
            }            
            else
            {
                type = "meta";
            }

            return new HtmlString($"<span class='m-badge m-badge--{type} m-badge--wide'>{status.ToDisplayName()}</span>");
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
