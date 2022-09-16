using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Core.Domain.Enums;

namespace Core.Appliaction.DTOs
{
    public class ReminderDto
    {
        public Guid Id { get; set; }
        public Guid userId { get; set; }
        public UserDto UserDto { get; set; }
        public string RemindFor { get; set; }
        public ReminderType ReminderType { get; set; }
        public ReminderStatus ReminderStatus { get; set; }
        public int? ReminderDays { get; set; }
        public ICollection<TaskDto> Tasks { get; set; }
    }
}
