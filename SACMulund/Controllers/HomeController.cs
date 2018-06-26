
using MimeKit;
using SACMulund.Models;
using System.Web.Configuration;
using System.Web.Mvc;
using MailKit.Net.Smtp;
using System;
using MailKit.Security;
using System.Net;
using MimeKit.Text;

namespace SACMulund.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> sendmail(string name, string email, string number)
        {
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(number))
            {
                EmailConfig ec = new EmailConfig
                {
                    FromAddress = WebConfigurationManager.AppSettings["FromAddress"],
                    FromName = WebConfigurationManager.AppSettings["FromName"],
                    MailServerAddress = WebConfigurationManager.AppSettings["MailServerAddress"],
                    MailServerPort = WebConfigurationManager.AppSettings["MailServerPort"],
                    UserId = WebConfigurationManager.AppSettings["UserId"],
                    UserPassword = WebConfigurationManager.AppSettings["UserPassword"]
                };

                try
                {
                    var emailMessage = new MimeMessage();

                    emailMessage.From.Add(new MailboxAddress(ec.FromName, ec.FromAddress));
                    emailMessage.To.Add(new MailboxAddress(ec.FromName, ec.FromAddress));
                    emailMessage.Subject = "Contact request";
                    emailMessage.Body = new TextPart(TextFormat.Html) { Text = "<html><body><table><tr style='font-size:18px;'><td colspan='2'>Contact request detail</td></tr><tr><tr><td>Name:</td><td>" + name + "</td></tr><tr><td>Email:</td><td>" + email  + "</td></tr><tr><td>Phone no:</td><td>" + number + "</td></tr></table></body></html>" };

                    using (var client = new SmtpClient())
                    {
                        var credentials = new NetworkCredential
                        {
                            UserName = ec.UserId,
                            Password = ec.UserPassword
                        };

                        //client.LocalDomain = "xyz.dk";
                        await client.ConnectAsync(ec.MailServerAddress, Convert.ToInt32(ec.MailServerPort), SecureSocketOptions.Auto).ConfigureAwait(false);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");

                        await client.AuthenticateAsync(credentials);

                        await client.SendAsync(emailMessage);
                        await client.DisconnectAsync(true).ConfigureAwait(false);
                        //You need to add return here
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return Json(true);
        }
    }
}