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
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;

        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }


        [HttpGet("GetAllRemindersByStatus")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(BaseResponse))]
        public async Task<IActionResult> GetAllRemindersByStatus([FromQuery] ReminderStatus status)
        {
            var response = await _reminderService.GetAllRemindersByStatusAsync(status);
            return Ok(response);
        }
    }
}
