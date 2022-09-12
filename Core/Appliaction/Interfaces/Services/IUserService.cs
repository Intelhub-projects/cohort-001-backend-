using Application.DTOs;
using Application.DTOs.RequestModels;
using Application.DTOs.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<BaseResponse> AddUserAsync(CreateUser user);

        public Task<BaseResponse> AddRoleAsync(CreateRole role);
        public Task<AdminUserResponseModel> GetUserAsync(string name);
        public Task<List<AdminUserResponseModel>> GetUsersAsync();
        public Task<AdminRoleResponseModel> GetRoleAsync(string name);
        public Task<List<AdminRoleResponseModel>> GetRolesAsync();
        public Task<IList<UsersInRoleResponseModel>> GetUsersByRoleAsync(string roleName);

        public Task<BaseResponse> CreatePatient(CreatePatient request);
    }
}
