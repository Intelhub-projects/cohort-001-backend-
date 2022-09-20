using Application.DTOs;
using Core.Appliaction.Interfaces.Services;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using RestfulAPI.Filters;
using System.Net;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailController: ControllerBase
    {

        private readonly IMailService _mailService;

        public SendMailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("SendMail")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(BaseResponse))]
        public  Task<IActionResult> SendMail([FromBody] MailRequest mailRequest)
        {
            _mailService.SendEmail(mailRequest);
            return Ok(new BaseResponse { Status = true, Message "Emaill successfully sent"})
            
        }
    }

    
}
