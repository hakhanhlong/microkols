
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

namespace WebMerchant.Code.TagHelpers
{
    public class AlertTagHelper : TagHelper
    {
        private const string AlertKey = "AppAlert";
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        protected ITempDataDictionary TempData => ViewContext.TempData;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            if (TempData[AlertKey] == null)
                TempData[AlertKey] = JsonConvert.SerializeObject(new HashSet<Alert>());
            var alerts = JsonConvert.DeserializeObject<ICollection<Alert>>(TempData[AlertKey].ToString());
            var html = string.Empty;
            foreach (var alert in alerts)
            {
                html += $"<div class='alert {alert.Type}'>" +
                $"<button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
                $"<span aria-hidden='true'>&times;</span>" +
                $"</button>" +
                $"{alert.Message}" +
                $"</div>";
            }
            output.Content.SetHtmlContent(html);
        }
    }
    public class Alert
    {
        public string Message;
        public string Type;
        public Alert(string message, string type)
        {
            Message = message;
            Type = type;
        }
    }
   
}
