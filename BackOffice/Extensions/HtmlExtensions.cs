﻿using Core.Entities;
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

        public static HtmlString ToBadge(this Core.Entities.CampaignAccountStatus status)
        {
            var type = "primary";
            if(status == CampaignAccountStatus.Finished)
            {
                type = "success";
            }

            if (status == CampaignAccountStatus.AccountRequest)
            {
                type = "brand";
            }

            if (status == CampaignAccountStatus.AgencyRequest)
            {
                type = "primary";
            }

            if (status == CampaignAccountStatus.Confirmed)
            {
                type = "info";
            }

            if (status == CampaignAccountStatus.SubmittedContent)
            {
                type = "accent";
            }

            if (status == CampaignAccountStatus.DeclinedContent)
            {
                type = "focus";
            }

            if (status == CampaignAccountStatus.ApprovedContent || status == CampaignAccountStatus.DeclinedContent)
            {
                type = "focus";
            }

            if (status == CampaignAccountStatus.Canceled)
            {
                type = "warning";
            }

            if (status == CampaignAccountStatus.NeedToCheckExcecuteCampaign)
            {
                type = "warning";
            }

            if (status == CampaignAccountStatus.AgencyCanceled)
            {
                type = "danger";
            }

            if (status == CampaignAccountStatus.Unfinished)
            {
                type = "danger";
            }



            return new HtmlString($"<span class='m-badge m-badge--{type} m-badge--wide'>{status.ToShowName()}</span>");


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

            return new HtmlString($"<span class='m-badge m-badge--{type} m-badge--wide'>{status.ToShowName()}</span>");
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

            return new HtmlString($"<span class='m-badge m-badge--{type} m-badge--wide'>{status.ToShowName()}</span>");
        }

        public static HtmlString ToBadge(this Core.Entities.TransactionType ttype)
        {
            var type = "primary";
            if (ttype == TransactionType.CampaignAccountCharge)
            {
                type = "warning";
            }
            else if (ttype == TransactionType.CampaignAccountPayback)
            {
                type = "focus";
            }
            else if (ttype == TransactionType.WalletRecharge)
            {
                type = "info";
            }

            else if (ttype == TransactionType.CampaignServiceCashBack)
            {
                type = "accent";
            }
            else if (ttype == TransactionType.CampaignServiceCharge)
            {
                type = "success";
            }
            else
            {
                type = "meta";
            }

            return new HtmlString($"<span class='m-badge m-badge--{type} m-badge--wide'>{ttype.ToShowName()}</span>");
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
