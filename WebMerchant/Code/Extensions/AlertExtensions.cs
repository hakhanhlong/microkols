using WebMerchant.Code.TagHelpers;
using WebServices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class AlertExtensions
    {
        private const string AlertKey = "AppAlert";
        public static void AddAlertSuccess(this Controller page, string message)
        {
            var alerts = GetAlerts(page);
            alerts.Add(new Alert(message, "alert-success"));
            page.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
        }
        public static void AddAlertInfo(this Controller page, string message)
        {
            var alerts = GetAlerts(page);
            alerts.Add(new Alert(message, "alert-info"));
            page.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
        }
        public static void AddAlertWarning(this Controller page, string message)
        {
            var alerts = GetAlerts(page);
            alerts.Add(new Alert(message, "alert-warning"));
            page.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
        }
        public static void AddAlertDanger(this Controller page, string message)
        {
            var alerts = GetAlerts(page);
            alerts.Add(new Alert(message, "alert-danger"));
            page.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
        }

        public static void AddAlert(this Controller page, bool r, string message = "")
        {
            if (r)
            {
                if (string.IsNullOrEmpty(message))
                {

                    message = "Cập nhật thông tin thành công";
                }
                page.AddAlertSuccess(message);
            }
            else
            {
                if (string.IsNullOrEmpty(message))
                {

                    message = "Lỗi khi Cập nhật thông tin. Xin vui lòng thử lại";
                }
                page.AddAlertDanger(message);
            }
        }

        private static ICollection<Alert> GetAlerts(Controller page)
        {
            if (page.TempData[AlertKey] == null)
                page.TempData[AlertKey] = JsonConvert.SerializeObject(new HashSet<Alert>());
            ICollection<Alert> alerts = JsonConvert.DeserializeObject<ICollection<Alert>>(page.TempData[AlertKey].ToString());
            if (alerts == null)
            {
                alerts = new HashSet<Alert>();
            }
            return alerts;
        }
    }

}
