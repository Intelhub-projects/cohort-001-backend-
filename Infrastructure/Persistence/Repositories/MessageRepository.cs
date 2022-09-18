using Core.Appliaction.DTOs;
using Core.Appliaction.Interfaces.Repository;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllMessages()
        {
            var messages = await _context.Messages.Select(message => new Message
            {
                Text = message.Text,
                MessageType = message.MessageType,
                

            }).ToListAsync();

            return messages;
        }

        public Task<Message> GetMessageByType(MessageType messageType)
        {
            var messages = _context.Messages.Where(type => type.MessageType == messageType).Select(message => new Message
            {

                Text = message.Text,
                MessageType = message.MessageType

            });

            return (Task<Message>)messages;
            
        }
    }
}
