using Application.DTOs;
using Core.Appliaction.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Appliaction.Interfaces.Services
{
    public interface IMailService
    {
        public Task<BaseResponse> SendEmail(MailRequestDto mailRequest);

    }
}
