using Application.DTOs;
using Core.Appliaction.Interfaces.Services;
using Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using RestfulAPI.Filters;
using System.Net;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController: ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }


        [HttpGet("GetAllMessages")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(BaseResponse))]
        public async Task<IActionResult> GetAllMessages()
        {
            var response = await _messageService.GetAllMessages();
            return Ok(response);
        }

        [HttpGet("GetMessageByType")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(BaseResponse))]
        public async Task<IActionResult> GetMessageByType([FromQuery] MessageType messageType)
        {
            var response = await _messageService.GetMessageByType(messageType);
            return Ok(response);
        }
    }
}
