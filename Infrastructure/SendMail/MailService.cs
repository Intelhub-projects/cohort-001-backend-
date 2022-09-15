using Application.DTOs;
using Core.Appliaction.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SendMail
{
    public class MailService : IMailService
    {
        public Task<BaseResponse> SendMail(string text, string recipientMail)
        {
            throw new NotImplementedException();
        }
    }
}
