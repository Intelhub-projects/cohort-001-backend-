using Application.DTOs.ResponseModels;
using Application.Wrapper;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public IEnumerable<Role> Roles { get; set; } = new List<Role>();

    }

    public class RegisterUserRequestModel
    {
        [Required]
        public string UserName { get; set; }
         
        [Required]
        public string Password { get; set; }

        public IList<Guid> Roles { get; set; } = new List<Guid>();

    }

    public class UpdateUserRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public IList<Guid> Roles { get; set; } = new List<Guid>();
    }

    public class LoginRequestModel
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseModel : BaseResponse
    {
        public LoginResponseData Data { get; set; }
    }

    public class UserResponseModel : BaseResponse
    {
        public UserDto Data { get; set; }
    }

    public class AdminUserResponseModel : BaseResponse
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<RoleDto> Roles { get; set; } = new List<RoleDto>();
    }

    public class UsersInRoleResponseModel : BaseResponse
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string RoleName { get; set; }
        public IEnumerable<RoleDto> Roles { get; set; } = new List<RoleDto>();
    }

    public class UsersResponseModel : BaseResponse
    {
        public PaginatedList<UserDto> Data { get; set; }
    }

    public class LoginResponseData
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();

    }
}
