using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Core.Domain.Entities;

namespace Core.Appliaction.Interfaces.Repository
{
    public interface ITaskRepository : IRepository<MyTask>
    {

    }
}
