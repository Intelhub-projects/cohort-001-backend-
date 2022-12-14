using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Appliaction.Interfaces.Services
{
    public interface IMailAddressVerificationService
    {
        Task<BaseResponse> VerifyMailAddress(string emailAddress);
    }
}
