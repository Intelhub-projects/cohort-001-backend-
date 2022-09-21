using Application.Constants;
using Application.DTOs;
using Application.DTOs.Filters;
using Application.DTOs.RequestModels;
using Application.DTOs.ResponseModels;
using Application.Exceptions;
using Application.Interfaces.Identity;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Wrapper;
using Core.Appliaction.DTOs;
using Core.Appliaction.Interfaces.Repository;
using Core.Appliaction.Interfaces.Services;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IIdentityService _identityService;
        private readonly IUserRepository _userRepository;
        private readonly IMailAddressVerificationService _mailAddressVerificationService;
        private readonly IResponseService _responseService;
        private readonly IMailService _mailService;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IIdentityService identityService,
            IUserRepository userRepository, IMailAddressVerificationService mailAddressVerificationService,
              IResponseService responseService, IMailService mailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityService = identityService;
            _userRepository = userRepository;
            _mailAddressVerificationService = mailAddressVerificationService;
            _responseService = responseService;
            _mailService = mailService;
        }



        public async Task<BaseResponse> AddUserAsync(CreateUser request)
        {
            var userNameExist = await _userRepository.AnyAsync(u => u.UserName == request.UserName);
            if (userNameExist)
            {
                throw new BadRequestException(UsersConstant.AlreadyExist);
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName,
                Email = request.Email,
                Address = request.Address,
                Gender = request.Gender,

            };
            user.Password = _identityService.GetPasswordHash(request.Password);
            var newUser = await _userManager.CreateAsync(user);
            if (newUser == null)
            {
                throw new Exception(UsersConstant.NotSuccessMessage);
            }
            var result = await _userManager.AddToRolesAsync(user, request.RoleNames);
            return new BaseResponse
            {
                Message = UsersConstant.SuccessMessage,
                Status = true
            };
        }

        public async Task<List<AdminUserResponseModel>> GetUsersAsync()
        {
            var users = _userManager.Users.ToList();
            return users.Select(x => new AdminUserResponseModel
            {
                Id = x.Id,
                UserName = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Roles = x.UserRoles.Select(r => new RoleDto
                {
                    RoleId = r.Role.Id,
                    Name = r.Role.Name
                }).ToList(),
                Status = true,
                Message = UsersConstant.SuccessMessage
            }).ToList(); ;
        }

        public async Task<IList<UsersInRoleResponseModel>> GetUsersByRoleAsync(string roleName)
        {
            var users = await _userRepository.GetUsersInRoleAsync(roleName);
            return users;
        }
        public async Task<AdminUserResponseModel> GetUserAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return new AdminUserResponseModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = user.UserRoles.Select(r => new RoleDto
                {
                    RoleId = r.Role.Id,
                    Name = r.Role.Name
                }).ToList(),
                Status = true,
                Message = UsersConstant.SuccessMessage
            };
        }

        public async Task<BaseResponse> AddRoleAsync(CreateRole request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var roleExist = await _roleManager.RoleExistsAsync(request.Name);
            if (roleExist)
            {
                throw new BadRequestException(UsersConstant.AlreadyExist);
            }
            var role = new Role
            {
                Name = request.Name
            };
            var newRole = await _roleManager.CreateAsync(role);
            if (!newRole.Succeeded)
            {
                throw new Exception(UsersConstant.NotSuccessMessage);
            }
            return new BaseResponse
            {
                Message = UsersConstant.SuccessMessage,
                Status = true
            };
        }

        public async Task<AdminRoleResponseModel> GetRoleAsync(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            return new AdminRoleResponseModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Status = true,
                Message = UsersConstant.SuccessMessage
            };
        }

        public async Task<List<AdminRoleResponseModel>> GetRolesAsync()
        {
            var roles = _roleManager.Roles.ToList().Select(r => new AdminRoleResponseModel
            {
                RoleId = r.Id,
                RoleName = r.Name,
                Status = true,
                Message = UsersConstant.SuccessMessage

            }).ToList();
            return roles;
        }

        public async Task<BaseResponse> CreatePatient(CreatePatient request)
        {
            var checkUserName = await _userRepository.AnyAsync(checkUserName => checkUserName.UserName == request.UserName);

            if (checkUserName)
            {
                throw new BadRequestException(UsersConstant.AlreadyExist);
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var patient = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName,
                Email = request.Email,
                Address = request.Address,
                Gender = request.Gender,


            };

            patient.Password = _identityService.GetPasswordHash(request.Password);

            var response = await _mailAddressVerificationService.VerifyMailAddress(patient.Email);
            if (!response.Status) return new BaseResponse
            {
                Message = "Invalid email address!",
                Status = false
            };
            var newPatient = await _userManager.CreateAsync(patient);


            var mailRequest = new MailRequestDto
            {
                ToEmail = patient.Email,
                ToName = patient.FirstName,
                HtmlContent = $"<html><body><div><h1> {DateTime.Now}, Dear {patient.FirstName}, " +
                $"Welcome to MedPharm Health Care</h1><div><h3>Your login details are " +
                $"Email: {patient.Email}, Password: {request.Password}. for more enquiries, visit www.intelhubmedpharmcare.com.  Regards, ADMIN.</h3><h5>MEDPHARM!-</h5><h6></h6></div></div>></body></html>"

            };
            await _mailService.SendEmail(mailRequest);
            await _responseService.SendResponse(patient.PhoneNumber,

                $"{DateTime.Now} " +
                $"Dear {patient.FirstName}, " +
                $"Welcome to MedPharm Health Care! " +
                $"Your login details are: " +
                $"Email: {patient.Email}, Password: {request.Password} " +
                $"for more enquiries, visit www.intelhubmedpharmcare.com " +
                $"Regards, " +
                $"ADMIN.");


            if (newPatient == null)
            {
                throw new Exception(UsersConstant.NotSuccessMessage);
            }
            var result = await _userManager.AddToRoleAsync(patient, "Patient");
            return new BaseResponse
            {
                Message = UsersConstant.SuccessMessage,
                Status = true
            };


        }
    }
}
