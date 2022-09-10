using Application.DTOs;
using Application.DTOs.RequestModels;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulAPI.Filters;
using System.Net;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IConfiguration configuration, IUserService userService, ILogger<UserController> logger)
        {
            _configuration = configuration;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("create")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CreateUser))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        public async Task<IActionResult> Create([FromBody] CreateUser model)
        {
            var response = await _userService.AddUserAsync(model);
            return Ok(response);
        }

        [HttpGet("GetbyName")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        public async Task<IActionResult> GetUser(string name)
        {
            var response = await _userService.GetUserAsync(name);
            return Ok(response);
        }


        [HttpGet("GetUsers")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetUsersAsync();
            return Ok(response);
        }

        [HttpPost("addrole")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        public async Task<IActionResult> CreateRole([FromBody] CreateRole model)
        {
            var response = await _userService.AddRoleAsync(model);
            return Ok(response);
        }

        [HttpGet("role")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        public async Task<IActionResult> GetRole(string name)
        {
            var response = await _userService.GetRoleAsync(name);
            return Ok(response);
        }

        [HttpGet("GetRoles")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        public async Task<IActionResult> GetRoles()
        {
            var response = await _userService.GetRolesAsync();
            return Ok(response);
        }

        [HttpGet("GetUsersByRole")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        public async Task<IActionResult> GetUsersByRole([FromQuery] string roleName)
        {
            var response = await _userService.GetUsersByRoleAsync(roleName);
            return Ok(response);
        }
    }
}
