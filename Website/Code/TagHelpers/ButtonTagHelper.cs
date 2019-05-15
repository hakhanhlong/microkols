using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Code.TagHelpers
{
    [HtmlTargetElement("button", Attributes = "glyphs-icon", ParentTag = "div")]
    public class ButtonTagHelper : TagHelper
    {
        public string GlyphsIcon { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass($"glyphicon glyphicon-{GlyphsIcon}");
            output.PreContent.AppendHtml(icon);
            output.PreContent.AppendHtml(" ");
        }
    }
}
