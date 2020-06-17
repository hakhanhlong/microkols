using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Common
{
    public class SendEmailHelpers
    {

        public const string SendgridKey = "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";

        public static async Task SendEmail(string from, string to, string subject, string plainTextContent, string htmlContent, string receiverName)
        {
            var apiKey = SendgridKey;
            var client = new SendGridClient(apiKey);
            var _from = new EmailAddress(from, "Microkol Support");
            var _subject = subject;
            var _to = new EmailAddress(to, receiverName);
            var _plainTextContent = plainTextContent;
            var _htmlContent = htmlContent;
            var msg = MailHelper.CreateSingleEmail(_from, _to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
