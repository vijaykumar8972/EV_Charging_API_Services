using EVCharging.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EVCharging.Utilities.Helpers
{
    public class EmailHelper
    {
        private readonly EmailConfig _emailConfig;
        private static Random random = new Random();
        private readonly ConfigHelper _configHelper;
        private readonly LogHelper _logHelper;
        public EmailHelper()
        {
            _configHelper = FoundationObject.FoundationObj.configHelper;
            _logHelper = FoundationObject.FoundationObj.logHelper;
        }
        public string SentEmail(string FromEmail, string FromPassword, string toEmail, string Message)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(FromEmail);
                message.To.Add(new MailAddress(toEmail));
                message.Subject = "VisualAppsFoundry Login Credentials";
                message.IsBodyHtml = true;
                message.Body = Message;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(FromEmail, FromPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                var Msg = "Mail Sent Successfully!";
                return Msg;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string CreateSAEmailMessage(string Email, string pass, string username)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Hi " + Email + " ,");
            sb.AppendLine("Your Account registered successfully.");
            sb.AppendLine("Username : " + Email + " or " + username);
            sb.AppendLine("Password : " + pass);
            return sb.ToString(); ;
        }

        public string CreateUserEmailMessage(string Email, string pass, string name, string username)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Hi " + name + " ,");
            sb.AppendLine("Your Account registered successfully.");
            sb.AppendLine("Username : " + Email + " or " + username);
            sb.AppendLine("Password : " + pass);
            return sb.ToString(); ;
        }
        public bool SendHtmlContentMail(string[] toemailAddresses, string subject, string emailContent, string cc = null, Attachment attachment = null)
        {
            try
            {

                var mail = new MailMessage();
                var smtpServer = new SmtpClient(_configHelper.AppSettings["EmailSettings:Server"]);
                smtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                mail.From = new MailAddress(_configHelper.AppSettings["EmailSettings:UserName"]);

                mail.To.Add(string.Join(",", toemailAddresses));
                if (!string.IsNullOrEmpty(cc))
                    mail.CC.Add(cc);
                mail.Subject = subject;
                mail.Body = emailContent;
                mail.IsBodyHtml = true;
                if (attachment != null)
                    mail.Attachments.Add(attachment);
                smtpServer.Port = Convert.ToInt32(_configHelper.AppSettings["EmailSettings:SmtpServerPort"]);
                smtpServer.Credentials = new System.Net.NetworkCredential(_configHelper.AppSettings["EmailSettings:UserName"],
                   _configHelper.AppSettings["EmailSettings:Password"]);
                smtpServer.EnableSsl = Convert.ToBoolean(_configHelper.AppSettings["EmailSettings:EnableSsl"]);
                smtpServer.Send(mail);
               
                return true;
            }
            catch (Exception ex)
            {
                _logHelper.LogError("SendHtmlContentMail", "EmailHelper", ex);
                return false;
            }
        }
    }
}
