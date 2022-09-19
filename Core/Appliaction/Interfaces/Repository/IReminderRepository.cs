using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Filters;
using Application.Interfaces.Repositories;
using Application.Wrapper;
using Core.Appliaction.DTOs;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Appliaction.Interfaces.Repository
{
    public interface IReminderRepository : IRepository<Reminder>
    {
        Task<IList<ReminderDto>> GetAllRemindersByStatusAsync(ReminderStatus status);
        Task<PaginatedList<ReminderDto>> GetAllUserReminderByStatusAsync(Expression<Func<Reminder, bool>> expression, PaginationFilter filter);

    }
}
