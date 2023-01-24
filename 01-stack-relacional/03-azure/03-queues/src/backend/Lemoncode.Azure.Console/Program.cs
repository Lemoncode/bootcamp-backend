

// See https://aka.ms/new-console-template for more information
using SendGrid.Helpers.Mail;
using SendGrid;

Console.WriteLine("Hello, World!");
await SendEmail();

static async Task SendEmail()
{
    var apiKey = "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";
    var client = new SendGridClient(apiKey);
    var from = new EmailAddress("santypr@gmail.com", "Lemoncode Testing");
    var subject = "Sending with SendGrid is Fun";
    var to = new EmailAddress("santiagoporras@outlook.com", "Lemoncode User");
    var plainTextContent = "and easy to do anywhere, even with C#";
    var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
    var response = await client.SendEmailAsync(msg);
    Console.WriteLine("Correo enviado");
}