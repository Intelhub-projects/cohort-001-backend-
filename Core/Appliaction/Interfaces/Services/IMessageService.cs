using Core.Appliaction.DTOs;
using Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Appliaction.Interfaces.Services
{
    public interface IMessageService
    {
        Task<MessageDto> GetMessageByType(MessageType messageType);
        Task<IEnumerable<MessageDto>> GetAllMessages();
    }
}
