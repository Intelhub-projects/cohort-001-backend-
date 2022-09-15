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
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public MailService(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        public async Task<BaseResponse> SendWelcomeMailToNewPatient(string text, User recipientMail)
        {
            while (true)
            {
                try
                {
                    var fetchMessageTemplate = await _messageService.GetMessageByType(Core.Domain.Enums.MessageType.RegistrationMessage);
                    var recipient = await _userService.GetRoleAsync("Patient");

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(
                        "[REPLACE WITH YOUR NAME]",
                        "[REPLACE WITH YOUR EMAIL]"
                    ));
                    message.To.Add(new MailboxAddress(
                        "[REPLACE WITH DESIRED TO NAME]",
                        "[REPLACE WITH DESIRED TO EMAIL]"
                    ));
                    message.Subject = "Sending with Twilio SendGrid is Fun";
                    var bodyBuilder = new BodyBuilder
                    {
                        TextBody = "and easy to do anywhere, especially with C#",
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
