using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Appliaction.Interfaces.Repository;
using Core.Domain.Entities;
using Persistence.Context;
using Persistence.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class TaskRepository : BaseRepository<MyTask>, ITaskRepository
    {
        public TaskRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}
