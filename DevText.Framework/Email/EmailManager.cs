using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Globalization;
using System.Web.Mvc;
using DevText.Framework.Setting;
using DevText.Framework.Logging;
using DevText.Framework.Security;
using Castle.Core.Logging;

namespace DevText.Framework.Email
{
    public class EmailManager
    {
        private DirectoryInfo _rootDirectory;
        private ISite _site;
        private ILogger _logger;

        public EmailManager(DirectoryInfo rootDirectory,ISite site)
        {
            _rootDirectory = rootDirectory;
            _site = site;
            _logger = Log4netLogger.Instance;
        }

        private ISite Site
        {
            get { return _site; }
        }

        private EmailTemplate GetMailTemplate(string name)
        {
            var file = new FileInfo(Path.Combine(_rootDirectory.FullName, name + ".txt"));
            if (!file.Exists)
                _logger.Info(name + " mail template could not found.");
            return new EmailTemplate(file);
        }

        public void SendRegistrationMail(IUser user)
        {
            var template = GetMailTemplate(EmailTemplate.Registered);
            template.AssignVariable("SiteName", Site.SiteName);
            template.AssignVariable("UserName", user.UserName);
            template.AssignVariable("Password", user.Password);
            string activationUrl = VirtualPathUtility.Combine(Site.SiteUrl.ToString(), "/Account/Activate" + user.Id);
            template.AssignVariable("ActivationUrl", activationUrl);

            template.AssignVariable("SiteUrl", Site.SiteUrl.ToString());
            MailMessage message = template.Execute();
            message.To.Add(new MailAddress(user.Email));
            message.From = new MailAddress(Site.Noreplayemail);
        }

        public void SendForgotUsernameMail(IUser user)
        {
            var template = GetMailTemplate(EmailTemplate.Registered);
            template.AssignVariable("SiteName", Site.SiteName);
            template.AssignVariable("UserName", user.UserName);
            template.AssignVariable("Password", user.Password);
            template.AssignVariable("SiteUrl", Site.SiteUrl.ToString());
            
            MailMessage message = template.Execute();
            message.To.Add(new MailAddress(user.Email));
            message.From = new MailAddress(Site.Noreplayemail);
   
        }

        public void SendResetPasswordMail(IUser user)
        {
            var template = GetMailTemplate(EmailTemplate.Registered);
            template.AssignVariable("SiteName", Site.SiteName);
            template.AssignVariable("UserName", user.UserName);
            template.AssignVariable("Password", user.Password);
            MailMessage message = template.Execute();
            message.To.Add(new MailAddress(user.Email));
            message.From = new MailAddress(Site.Noreplayemail);
   
        }

        public void SendAccountActivatedMail(IUser user)
        {
            var template = GetMailTemplate(EmailTemplate.Registered);
            template.AssignVariable("SiteName", Site.SiteName);
            template.AssignVariable("UserName", user.UserName);
            template.AssignVariable("Password", user.Password);
            template.AssignVariable("SiteUrl", Site.SiteUrl.ToString());

            MailMessage message = template.Execute();
            message.To.Add(new MailAddress(user.Email));
            message.From = new MailAddress(Site.Noreplayemail);
   
        }

        public void SendEmail(MailMessage message)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Send(message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
            }
        }
    }
}
