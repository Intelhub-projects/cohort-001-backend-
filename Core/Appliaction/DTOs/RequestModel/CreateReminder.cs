using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Enums;

namespace Core.Appliaction.DTOs.RequestModel
{
    public class CreateReminder
    {
        public string RemindFor { get; set; }
        public ReminderType ReminderType { get; set; }
        public int? ReminderDays { get; set; }
        public ICollection<TaskDto> Tasks { get; set; }
    }
}
