using Application.DTOs;
using Application.Interfaces.Services;
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
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public MailService(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        public Task<BaseResponse> SendMail(string text, string recipientMail)
        {
            throw new NotImplementedException();
        }
    }
}
