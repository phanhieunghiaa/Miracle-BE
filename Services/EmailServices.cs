using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;

        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string toEmail, string subject, string content)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = content };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmail_ConfirmCode(string toEmail, string userName, string confirmCode)
        {
            string mailBox = _configuration.GetValue<string>("EmailSettings:FromEmail");
            string contentTemplate = "<p>Dear <strong>[User Name]</strong>,</p>\r\n    " +
                "<p>Thank you for registering with our service. To complete your registration, please use the following confirmation code:</p>\r\n    " +
                "<p><strong>Confirmation Code:</strong></p>\r\n    <p><strong>[Confirmation Code]</strong></p>\r\n " +
                "<br><p>If you did not request this code or have any questions, please contact us at: [Email].</p>\r\n    " +
                "<p>Thank you for choosing our service. We look forward to serving you.</p>\r\n    <p>Sincerely,</p>\r\n Miracle</p>";

            contentTemplate = Regex.Replace(contentTemplate, @"\[User Name\]", userName);
            contentTemplate = Regex.Replace(contentTemplate, @"\[Confirmation Code\]", confirmCode);
            contentTemplate = Regex.Replace(contentTemplate, @"\[Email]", mailBox);
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Verification Code";

            var bodyBuilder = new BodyBuilder { HtmlBody = contentTemplate };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
