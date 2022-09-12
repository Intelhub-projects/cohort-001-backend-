using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Appliaction.Interfaces.Repository;
using Core.Domain.Entities;
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
    }
}
