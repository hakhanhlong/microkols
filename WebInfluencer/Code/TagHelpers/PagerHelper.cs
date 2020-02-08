using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;
using WebServices.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;
using Microsoft.AspNetCore.Http;
using WebInfluencer.Code.Extensions;

namespace WebInfluencer.Code.TagHelpers
{

    [HtmlTargetElement("pager")]
    public class PagerHelper : AnchorTagHelper
    {
        
        public PagerHelper(IHtmlGenerator generator) : base(generator)
        {
           
        }

        public string PageParam { get; set; } = "pageindex";
        public PagerViewModel Pager { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            //var _routeValues = HttpContextAccessor.HttpContext.Request.Query.Where(m => m.Key != PageParam).ToList();


            int adjacents = 3;
            var prevtag = new TagBuilder("i");
            prevtag.AddCssClass("fas fa-chevron-left");

            var nexttag = new TagBuilder("i");
            nexttag.AddCssClass("fas fa-chevron-right");

            var result = new StringBuilder();

            if (Pager.TotalPages > 1)
            {
                var page = Pager.Page;
                var tpages = Pager.TotalPages;
                var pmin = (page > adjacents) ? (page - adjacents) : 1;
                var pmax = (page < (tpages - adjacents)) ? (page + adjacents) : tpages;


                if (Pager.Page > 1)
                {

                    var li = new TagBuilder("li");
                    li.AddCssClass("page-item");

                    var backrouteValues = RouteValues;
                    if (backrouteValues.ContainsKey(PageParam))
                    {
                        backrouteValues.Remove(PageParam);
                    }

                    backrouteValues.Add(PageParam, (Pager.Page - 1).ToString());



                    var tagBack = Generator.GeneratePageLink(ViewContext, "", Page, PageHandler, Protocol, Host, Fragment, backrouteValues, null);
                    tagBack.AddCssClass("page-link");
                    tagBack.InnerHtml.AppendHtml(prevtag.GetString());
                    li.InnerHtml.AppendHtml(tagBack.GetString());

                    result.AppendLine(li.GetString());
                }



                for (var i = pmin; i <= pmax; i++)
                {
                    var li = new TagBuilder("li");
                    li.AddCssClass("page-item");

                    var routeValues = RouteValues;
                    if (routeValues.ContainsKey(PageParam))
                    {
                        routeValues.Remove(PageParam);
                    }
                    routeValues.Add(PageParam, i.ToString());


                    var tag = Generator.GeneratePageLink(ViewContext, i.ToString(), Page, PageHandler, Protocol, Host, Fragment, routeValues, null);
                    tag.AddCssClass("page-link");
                    li.InnerHtml.AppendHtml(tag.GetString());
                    if (i == Pager.Page)
                    {
                        li.AddCssClass("active");
                    }
                    var item = li.GetString();
                    result.AppendLine(item);
                }

                if (Pager.Page < Pager.TotalPages)
                {

                    var li = new TagBuilder("li");
                    li.AddCssClass("page-item");

                    var nextrouteValues = RouteValues;

                    if(nextrouteValues.ContainsKey(PageParam))
                    {
                        nextrouteValues.Remove(PageParam);
                    }
                    nextrouteValues.Add(PageParam, (Pager.Page + 1).ToString());

                    var tagNext = Generator.GeneratePageLink(ViewContext, "", Page, PageHandler, Protocol, Host, Fragment, nextrouteValues, null);
                    tagNext.AddCssClass("page-link");
                    tagNext.InnerHtml.AppendHtml(nexttag.GetString());
                    li.InnerHtml.AppendHtml(tagNext.GetString());

                    result.AppendLine(li.GetString());
                }
            }
            var html = result.ToString();
            output.TagName = "ul";
            output.Attributes.Add("class", "pagination justify-content-end");
            output.Content.SetHtmlContent(html);
            output.TagMode = TagMode.StartTagAndEndTag;

        }

    }
}