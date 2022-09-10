using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ResponseModels
{
    public class RoleDto
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
    }

    public class RolesResponseModel : BaseResponse
    {
        public IList<Role> Data { get; set; }
    }

    public class AdminRoleResponseModel : BaseResponse
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
