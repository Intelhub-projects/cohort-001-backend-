using Application.Interfaces.Repositories;
using Core.Appliaction.DTOs;
using Core.Domain.Entities;
using Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Appliaction.Interfaces.Repository
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessageByType(MessageType messageType);
        Task<IEnumerable<Message>> GetAllMessages();

    }
}
