using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;

namespace Core.Domain.Entities
{
    public class MyTask : AuditableEntity
    {
        public Guid ReminderId { get; set; }
        public Reminder Reminder { get; set; }
        public string Todo { get; set; }
        public DateTime TodoTime { get; set; }
    }
}
