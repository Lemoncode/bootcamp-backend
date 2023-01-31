using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.Azure.Fx.Services
{
    public class SendGridService
    {
        public async Task SendEmail()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("santypr@gmail.com", "Lemoncode Testing");
            var subject = "Sending with SendGrid from Azure Function is Fun";
            var to = new EmailAddress("santiagoporras@outlook.com", "Lemoncode User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            Console.WriteLine("Correo enviado");
        }
    }
}
