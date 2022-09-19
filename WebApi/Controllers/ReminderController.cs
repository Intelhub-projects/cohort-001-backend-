using Application.DTOs;
using Application.DTOs.Filters;
using Core.Appliaction.DTOs.RequestModel;
using Core.Appliaction.Interfaces.Services;
using Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using RestfulAPI.Filters;
using System.Net;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;

        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        [HttpPost("CreateReminderAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(BaseResponse))]
        public async Task<IActionResult> CreateReminderAsync([FromQuery] Guid userId, [FromBody] CreateReminder request)
        {
            var response = await _reminderService.CreateAsync(userId, request);
            return Ok(response);
        }


        [HttpGet("GetAllRemindersByStatusAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(BaseResponse))]
        public async Task<IActionResult> GetAllRemindersByStatusAsync([FromQuery] ReminderStatus status)
        {
            var response = await _reminderService.GetAllRemindersByStatusAsync(status);
            return Ok(response);
        }

        [HttpGet("GetUserOnboardRemindersByUserIdAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(BaseResponse))]
        public async Task<IActionResult> GetUserOnboardRemindersByUserIdAsync([FromQuery] Guid userId, [FromQuery]PaginationFilter filter)
        {
            var response = await _reminderService.GetOnboardReminderByUserIdAsync(userId, filter);
            return Ok(response);
        }

        [HttpGet("GetUserDoneRemindersByUserIdAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(BaseResponse))]
        public async Task<IActionResult> GetUserDoneRemindersByUserIdAsync([FromQuery] Guid userId, [FromQuery] PaginationFilter filter)
        {
            var response = await _reminderService.GetDoneReminderByUserIdAsync(userId, filter);
            return Ok(response);
        }
    }
}
