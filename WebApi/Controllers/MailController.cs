using Application.DTOs;
using Core.Appliaction.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using RestfulAPI.Filters;
using System.Net;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController: ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("SendWelcomeMailToNewPatient")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(BaseResponse))]
        public async Task<IActionResult> SendWelcomeMailToNewPatient([FromBody] string recipientName, string recipientMail, string text)
        {
            var response = await _mailService.SendWelcomeMailToNewPatient(recipientName, recipientMail, text);
            return Ok(response);
        }
    }
}
