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

        public Task<IEnumerable<ReminderDto>> GetAllRemindersByStatusAsync(ReminderStatus status)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedList<ReminderDto>> GetAllUserReminderByStatusAsync(Expression<Func<Reminder, bool>> expression, PaginationFilter filter)
        {
            var reminders = await _context.Reminders.Where(expression).Select(reminder => new ReminderDto
            {
                RemindFor = reminder.RemindFor,
                ReminderDays = reminder.ReminderDays,
                ReminderType = reminder.ReminderType,
                ReminderStatus = reminder.ReminderStatus,
                RemindDateAndTime = ConverToDateTime(reminder.RemindDateAndTime)
            }).AsNoTracking().ToPaginatedListAsync(filter.PageNumber, filter.PageSize);
            return reminders;
        }

        private ICollection<DateTime> ConverToDateTime(string remindDateAndTime)
        {
            var a = remindDateAndTime.Split(" ");
            List<DateTime> date = new List<DateTime>();
            foreach (var item in a)
            {
                date.Add(Convert.ToDateTime(item));
            }
            return date;
        }

        /*private ICollection<DateTime> ConvertToDateTime(string dateANDTime)
        {
            var a = dateANDTime.Split(" ");
            List<DateTime> date = new List<DateTime>();
            foreach(var item in a)
            {
                date.Add(Convert.ToDateTime(item));
            }
            return date;
        }*/
    }
}
