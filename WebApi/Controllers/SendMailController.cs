using Application.DTOs;
using Core.Appliaction.DTOs;
using Core.Appliaction.Interfaces.Services;
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
        public async Task <IActionResult> SendMail([FromBody] MailRequestDto mailRequest)
        {
            var response = await _mailService.SendEmail(mailRequest);
            return Ok(response);
            
        }
    }

    
}
