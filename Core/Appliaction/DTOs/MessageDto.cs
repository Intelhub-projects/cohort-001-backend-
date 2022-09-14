using Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Appliaction.DTOs
{
    public class MessageDto
    {
        public Guid UserId { get; set; }
        public string text { get; set; }
        public MessageType MessageType { get; set; }
    }
}
