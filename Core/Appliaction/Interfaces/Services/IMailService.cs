using Application.DTOs;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Appliaction.Interfaces.Services
{
    public interface IMailService
    {
        public Task<MailRequest> SendEmail(MailRequest mailRequest);

    }
}
