using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Appliaction.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public Guid ReminderId { get; set; }
        public string Todo { get; set; }
        public DateTime TodoTime { get; set; }
    }
}
