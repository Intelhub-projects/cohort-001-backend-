using Application.DTOs;
using Application.Interfaces.Services;
using Core.Appliaction.Interfaces.Services;
using Core.Domain.Entities;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SendMail
{
    public class MailService : IMailService
    {
        public async Task<BaseResponse> SendWelcomeMailToNewPatient(string recipientName, string recipientMail, string text)
        {
            while (true)
            {
                try
                {
                  
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(
                        "INTELHUB MEDPHARM CARE",
                        "greatmoh007@gmail.com"
                    ));

                    message.To.Add(new MailboxAddress(
                       $"{recipientName}",
                        $"{recipientMail}"
                    ));
                    message.Subject = "Because Your Life Matters";
                    var bodyBuilder = new BodyBuilder
                    {
                        TextBody = $"{text}",
                        HtmlBody = "and easy to do anywhere, <b>especially with C#</b>"
                    };
                    message.Body = bodyBuilder.ToMessageBody();

                    using var client = new SmtpClient();
                    // SecureSocketOptions.StartTls force a secure connection over TLS
                    await client.ConnectAsync("smtp.sendgrid.net", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(
                        userName: "apikey", // the userName is the exact string "apikey" and not the API key itself.
                        password: Environment.GetEnvironmentVariable("SENDGRID_API_KEY") // password is the API key
                    );

                    Console.WriteLine("Sending email");
                    await client.SendAsync(message);
                    Console.WriteLine("Email sent");

                    await client.DisconnectAsync(true);
                }
                catch (Exception)
                {

                }
            }
            
        }
    }
}
