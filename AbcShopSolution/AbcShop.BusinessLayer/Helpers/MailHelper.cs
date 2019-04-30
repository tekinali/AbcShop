using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AbcShop.BusinessLayer.Helpers
{
    public class MailHelper
    {
        public static bool SendMail(string body, string to, string subject, bool isHtml = true)
        {
            return SendMail(body, new List<string> { to }, subject, isHtml);
        }

        public static bool SendMail(string body, List<string> to, string subject, bool isHtml = true)
        {
            bool result = false;

            try
            {
                SmtpClient smtp = new SmtpClient(ConfigHelper.Get<string>("MailHost"),
                    ConfigHelper.Get<int>("MailPort"));
                smtp.Timeout = 10000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(ConfigHelper.Get<string>("MailUser"), ConfigHelper.Get<string>("MailPass"));
                smtp.EnableSsl = true;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(ConfigHelper.Get<string>("MailUser"));
                to.ForEach(x =>
                {
                    message.To.Add(new MailAddress(x));
                });
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                message.BodyEncoding = UTF8Encoding.UTF8;

                smtp.Send(message);

            }
            catch (Exception)
            {

            }

            return result;
        }
    }
}
