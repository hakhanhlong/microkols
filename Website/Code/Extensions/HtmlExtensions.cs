using Common.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Website.ViewModels;

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


        public static string UrlGetMatchedAccount(this IUrlHelper urlHelper, CampaignDetailsViewModel model, int type)
        {
            var queryParams = new Dictionary<string, string>();

            var parameters = new RouteValueDictionary();

            var accountTypes = type == 1 ? new List<AccountType>() { AccountType.Regular } : model.AccountTypes.Where(m => m != AccountType.Regular).ToList();

            for (int i = 0; i < accountTypes.Count; ++i)
            {
                parameters.Add("accountTypes[" + i + "]", accountTypes[i]);
            }
            if (model.CategoryIds.Count > 0)
            {
                for (int i = 0; i < model.CategoryIds.Count; ++i)
                {
                    parameters.Add("categoryid[" + i + "]", model.CategoryIds[i]);
                }
            }

            if (model.Gender.HasValue)
            {
                parameters.Add("gender", model.Gender.Value);
            }

            if (model.CityId.HasValue)
            {
                parameters.Add("cityid", model.CityId.Value);
            }
            if (model.AgeStart.HasValue && model.AgeEnd.HasValue)
            {
                parameters.Add("agestart", model.AgeStart.Value);
                parameters.Add("ageend", model.AgeEnd.Value);
            }

            if (model.CampaignAccounts.Count > 0)
            {
                for (int i = 0; i < model.CampaignAccounts.Count; ++i)
                {
                    parameters.Add("ignoreIds[" + i + "]", model.CampaignAccounts[i].Account.Id);
                }

            }
            /*
             IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender, int? cityid, int? agestart, int? ageend,

            string order, int page, int pagesize, IEnumerable<int> ignoreIds
             */

            return urlHelper.Action("MatchedAccount", "Account", parameters);

        }
    }


}
