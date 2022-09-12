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
using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IIdentityService identityService, IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityService = identityService;
            _userRepository = userRepository;
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
                UserName = request.UserName,
                Email = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                CreatedBy = "fmkDev"
                
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
            var roleExist = await _userRepository.AnyAsync(r => r.Name == request.Name);
            if(roleExist)
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
            var role= await _roleManager.FindByNameAsync(name);
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
                Password = request.Password,
                

            };

            patient.Password = _identityService.GetPasswordHash(patient.Password);
            var newPatient = await _userManager.CreateAsync(patient);
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
