using Application.DTOs;
using Application.Interfaces.Identity;
using Application.Interfaces.Services;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestfulAPI.Filters;
using System.Net;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IIdentityService _identityService;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration,  IUserService userService, ILogger<AuthController> logger, UserManager<User> userManager, IIdentityService identityService)
        {
            _configuration = configuration;
            _logger = logger;
            _identityService = identityService;
            _userManager = userManager;
            _userService = userService;
        }

        [HttpPost("token")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        public async Task<IActionResult> Token([FromBody] LoginRequestModel model)
        {
            User user;
            if (!string.IsNullOrEmpty(model.Password))
            {
                user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    var isValidPassword = _identityService.CheckPasswordAsync(user, model.Password);
                    if (!isValidPassword)
                    {
                        var failedResponse = new BaseResponse
                        {
                            Message = "Invalid Credential",
                            Status = false
                        };
                        return BadRequest(failedResponse);
                    }
                }
            }
            else
            {
                user = await _identityService.FindByNameAsync(model.UserName);
            }

            if (user != null)
            {
                var roles = await _identityService.GetRolesAsync(user);
                var token = _identityService.GenerateToken(user, roles);
                var expiry = DateTimeOffset.UtcNow.AddMinutes(Convert.ToInt32(_configuration.GetValue<string>("JwtTokenSettings:TokenExpiryPeriod")));
                var tokenResponse = new LoginResponseModel
                {
                    Message = "Login Successful",
                    Status = true,
                    Data = new LoginResponseData
                    {
                        Roles = roles,
                        UserName = user.UserName,
                        FirstName = user.Firstname,
                        LastName = user.LastName
                    }
                };

                Response.Headers.Add("Token", token);
                Response.Headers.Add("TokenExpiry", expiry.ToUnixTimeMilliseconds().ToString());
                Response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
                return Ok(tokenResponse);
            }
               
            var response = new BaseResponse
            {
                Message = "Invalid Credential",
                Status = false
            };
            return BadRequest(response);
        }

        [HttpPost("admin")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationResultModel))]
        public async Task<IActionResult> AdminLogin([FromBody] LoginRequestModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!isValidPassword)
                {
                    var failedResponse = new BaseResponse
                    {
                        Message = "Invalid Credential",
                        Status = false
                    };
                    return BadRequest(failedResponse);
                }
                var roles = await _userManager.GetRolesAsync(user);
                var token = _identityService.GenerateToken(user, roles);
                var expiry = DateTimeOffset.UtcNow.AddMinutes(Convert.ToInt32(_configuration.GetValue<string>("JwtTokenSettings:TokenExpiryPeriod")));
                var tokenResponse = new LoginResponseModel
                {
                    Message = "Login Successful",
                    Status = true,
                    Data = new LoginResponseData
                    {
                        Roles = roles,
                        UserName = user.UserName,
                        FirstName = user.Firstname,
                        LastName = user.LastName
                    }
                };

                Response.Headers.Add("Token", token);
                Response.Headers.Add("TokenExpiry", expiry.ToUnixTimeMilliseconds().ToString());
                Response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
                return Ok(tokenResponse);
            }
            var response = new BaseResponse
            {
                Message = "Invalid Credential",
                Status = false
            };
            return BadRequest(response);
        }
    }
}
