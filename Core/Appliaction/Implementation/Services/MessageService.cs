using Core.Appliaction.DTOs;
using Core.Appliaction.Interfaces.Repository;
using Core.Appliaction.Interfaces.Services;
using Core.Domain.Entities;
using Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Appliaction.Implementation.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IEnumerable<Message>> GetAllMessages()
        {
            return await _messageRepository.GetAllMessages();
        }

        public async Task<IEnumerable<Message>> GetMessageByType(MessageType messageType)
        {
           return await _messageRepository.GetMessageByType(messageType);

            
        }

        
    }
}
