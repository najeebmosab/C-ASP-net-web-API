using FinalProject.Datas.Edintites;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly IHostingEnvironment _host;

        public SendEmailController(IHostingEnvironment host)
        {
            _host = host;
        }


        [HttpPost]
        public void SendMassege()
        {
            using (MailMessage mail = new MailMessage()) 
            {
                mail.From = new MailAddress(HttpContext.Request.Form["From"]);
                mail.To.Add(HttpContext.Request.Form["To"]);
                mail.Subject = HttpContext.Request.Form["Subject"];
                var file = Path.GetFileName(HttpContext.Request.Form["File"]);
                mail.Attachments.Add(new Attachment(Path.Combine(_host.WebRootPath + "/images/products", file)));
                mail.Body = HttpContext.Request.Form["Body"]+ $"<br> <div class='form-group'> <abel for= 'ProductName' > Product Name </ label ><h2>{HttpContext.Request.Form["ProductName"]}</h2></div> <br> <div class='form-group'> <abel for= 'Description' > Product Description </ label ><h2>{HttpContext.Request.Form["Description"]}</h2></div> <br> <div class='form-group'> <abel for= 'Price' > Product Price </ label ><h2>${HttpContext.Request.Form["Price"]}</h2></div> <br> <div class='form-group'> <abel for= 'Quantity' > Product Quantity </ label ><h2>{HttpContext.Request.Form["Quantity"]}</h2></div> <br>";



                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new System.Net.NetworkCredential("najeebmosab@gmail.com", "kxcsdbkomerlicsx");
                smtp.EnableSsl = true;

                smtp.Send(mail);
                
            }
        }

        [Route("CheckCode")]
        [HttpPost]
        public string SendMassegeCheckCode()
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("najeebmosab@gmail.com");
                mail.To.Add(HttpContext.Request.Form["To"]);
                mail.Subject = "Code To Check Email";
                mail.Body = "";
                mail.Priority = MailPriority.High;
                var rand = new Random();
                string Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
                for (int i = 0; i < 4; i++)
                {
                    int ch = rand.Next(0, 62);
                    mail.Body += Code[ch];
                }

                Code = mail.Body;


                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("najeebmosab@gmail.com", "kxcsdbkomerlicsx");
                smtp.EnableSsl = true;

                smtp.Send(mail);

                return Code;
            }
        }
    }
}
