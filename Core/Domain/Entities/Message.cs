using Core.Domain.Enums;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class Message: AuditableEntity
    {
        public Guid UserId { get; set; }
        public string text { get; set; }
        public MessageType MessageType { get; set; }
    }
}
