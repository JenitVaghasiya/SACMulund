
using MimeKit;
using SACMulund.Models;
using System.Web.Configuration;
using System.Web.Mvc;
using System;
using MailKit.Security;
using System.Net;
using MimeKit.Text;
using System.Net.Mail;

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
        public JsonResult sendmail(string name, string email, string number)
        {
            try
            {
                EmailConfig ec = new EmailConfig
                {
                    FromAddress = WebConfigurationManager.AppSettings["FromAddress"],
                    FromName = WebConfigurationManager.AppSettings["FromName"],
                    ToAddress = WebConfigurationManager.AppSettings["ToAddress"]
                };


                const string SERVER = "relay-hosting.secureserver.net";
                MailMessage oMail = new System.Net.Mail.MailMessage();
                oMail.From = new System.Net.Mail.MailAddress(ec.FromAddress);
                oMail.To.Add(new System.Net.Mail.MailAddress(ec.ToAddress));
                oMail.Subject = ec.FromName;// email's subject
                oMail.Priority = System.Net.Mail.MailPriority.High;
                oMail.Body = "<html><body><table><tr style='font-size:18px;'><td colspan='2'>Contact request detail</td></tr><tr><tr><td>Name:</td><td>" + name + "</td></tr><tr><td>Email:</td><td>" + email + "</td></tr><tr><td>Phone no:</td><td>" + number + "</td></tr></table></body></html>";
                oMail.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient(SERVER);
                smtpClient.Port = 25;
                smtpClient.Send(oMail);
                oMail = null;    // free up resources
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(true);
        }
    }
}