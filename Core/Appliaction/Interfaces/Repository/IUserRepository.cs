using Application.DTOs;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<IList<UsersInRoleResponseModel>> GetUsersInRoleAsync(string roleName);
        public Task<bool> AnyAsync(Expression<Func<Role, bool>> expression);
        public Task<Role> GetAsync(Expression<Func<Role, bool>> expression);
        public Task<User> GetUserAndRoles(string userName);
    }
}
