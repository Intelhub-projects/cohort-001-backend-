using Application.DTOs;
using Application.DTOs.ResponseModels;
using Application.Interfaces.Repositories;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UserRepository :  BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> AnyAsync(Expression<Func<User, bool>> expression)
        {
            return await _context.Users.AnyAsync(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<Role, bool>> expression)
        {
            return await _context.Roles.AnyAsync(expression);
        }

        public async Task<User> GetAsync(Expression<Func<User, bool>> expression)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<Role> GetAsync(Expression<Func<Role, bool>> expression)
        {
            return await _context.Roles.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<User> GetUserAndRoles(string userName)
        {
            var user = await _context.Users.Include(r => r.UserRoles)
                .ThenInclude(r => r.Role).FirstOrDefaultAsync(u => u.UserName == userName);
            return user;
        }

        public async Task<IList<UsersInRoleResponseModel>> GetUsersInRoleAsync(string roleName)
        {
            var userRoles = await _context.UserRoles.Include(u => u.User)
                .Include(u => u.Role)
                .AsNoTracking()
                .Where(u => u.Role.Name == roleName)
                .Select(x => new UsersInRoleResponseModel
                {
                    Id = x.Id,
                    UserName = x.User.UserName,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Roles = x.User.UserRoles.Select(u => new RoleDto
                    {
                        RoleId = u.Role.Id,
                        Name = u.Role.Name,
                    }).ToList(),
                    Status = true,
                    Message = "Success"
                })
                .ToListAsync();

            return userRoles;
        }
    }
}
