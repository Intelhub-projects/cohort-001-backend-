using Application.DTOs;
using Application.Interfaces.Services;
using Core.Appliaction.Interfaces.Services;
using Core.Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        public string _mailKey;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _mailKey = _configuration.GetSection("MailConfiguration")["mailKey"];
        }

        public async Task<BaseResponse> SendWelcomeMailToNewPatient(string recipientName, string recipientMail, string text)
        {





        }
    }   }
