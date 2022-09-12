using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Filters;
using Application.Wrapper;
using Core.Appliaction.DTOs;
using Core.Appliaction.Interfaces.Repository;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Persistence.Context;
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

        public Task<PaginatedList<ReminderDto>> GetAllUserReminderByStatusAsync(Guid userId, PaginationFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
