using AutoMapper;
using AutoMapper.QueryableExtensions;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.MessageRepo
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
         public void AddMessage(Message message)
        {
             _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                .Include(u=>u.Sender)
                .Include(u=>u.Recipient)
                .SingleOrDefaultAsync(x=>x.MessageId==id);
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesForUserAsync(MessageParams messageParams)
        {
            var query = _context.Messages
                 .OrderByDescending(m => m.SentAt)
                 .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUserName == messageParams.Username && u.RecipientDeleted == false ),
                "Outbox" => query.Where(u => u.SenderUserName == messageParams.Username && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUserName == messageParams.Username && u.DateRead == null && u.RecipientDeleted==false )
            };

            var messages = query.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider);
            return await messages.ToListAsync();
        }

        public async Task<IEnumerable<MessageDTO>> GetMessageThreadAsync(string currentUsername, 
            string recipientUsername)
        {
            var messages=await _context.Messages
                .Include(u=>u.Sender)
                .Include(u=>u.Recipient)
                .Where(m=>m.Recipient.UserName==currentUsername && m.RecipientDeleted==false
                && m.Sender.UserName== recipientUsername
                || m.Recipient.UserName==recipientUsername
                && m.Sender.UserName==currentUsername && m.SenderDeleted==false
                )
                .OrderBy(m=>m.SentAt)
                .ToListAsync();

            var unreadMessages=messages.Where(m=>m.DateRead==null 
            && m.Recipient.UserName==currentUsername).ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.Now;
                }
                await _context.SaveChangesAsync();  
            }
            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }

        public async Task<IEnumerable<MessageUserDTO>> GetChattedUsersAsync(string username)
        {
            // Step 1: Fetch all relevant messages
            var messages = await _context.Messages
                .Where(m => m.SenderUserName == username || m.RecipientUserName == username)
                .ToListAsync();

            // Step 2: Get the distinct users involved in the messages
            var userIds = messages
                .Select(m => m.SenderUserName == username ? m.RecipientId : m.SenderId)
                .Distinct()
                .ToList();

            // Step 3: Fetch user details for the distinct user IDs
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ProjectTo<MessageUserDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return users;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
