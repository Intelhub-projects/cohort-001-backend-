using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Filters;
using Application.Wrapper;
using Core.Appliaction.DTOs;
using Core.Appliaction.Interfaces.Repository;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using Persistence.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class ReminderRepository : BaseRepository<Reminder>, IReminderRepository
    {
        public ReminderRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IList<ReminderDto>> GetAllRemindersByStatusAsync(ReminderStatus status)
        {
            var reminders = await _context.Reminders.Select(reminder => new ReminderDto
            {
                ReminderDays = reminder.ReminderDays,
                ReminderStatus = reminder.ReminderStatus,
                ReminderType = reminder.ReminderType,
                RemindFor = reminder.RemindFor,
                userId = reminder.UserId,
                Tasks = reminder.Tasks.Select(task => new TaskDto
                {
                    Id = task.Id,
                    Todo = task.Todo,
                    TodoTime = task.TodoTime,
                }).ToList()
            }).ToListAsync();

            return reminders;
        }

        public async Task<PaginatedList<ReminderDto>> GetAllUserReminderByStatusAsync(Expression<Func<Reminder, bool>> expression, PaginationFilter filter)
        {
            var reminders = await _context.Reminders.Where(expression)
                .Select(reminder => new ReminderDto
            {
                RemindFor = reminder.RemindFor,
                ReminderDays = reminder.ReminderDays,
                ReminderType = reminder.ReminderType,
                ReminderStatus = reminder.ReminderStatus,
                Tasks = reminder.Tasks.Select(task => new TaskDto
                {
                    Id = task.Id,
                    Todo = task.Todo,
                    TodoTime = task.TodoTime,
                }).ToList()
            }).AsNoTracking().ToPaginatedListAsync(filter.PageNumber, filter.PageSize);
            return reminders;
        }


    }
}
