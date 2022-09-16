using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Enums;
using Domain.Contracts;

namespace Core.Domain.Entities
{
    public class Reminder : AuditableEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string RemindFor { get; set; }
        public ReminderType ReminderType { get; set; }
        public ReminderStatus ReminderStatus { get; set; }
        public int? ReminderDays { get; set; }
        public ICollection<MyTask> Tasks { get; set; } = new HashSet<MyTask>();
    }
}
