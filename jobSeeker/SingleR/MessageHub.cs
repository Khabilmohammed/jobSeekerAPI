using AutoMapper;
using jobSeeker.DataAccess.Data.Repository.IUserRepository;
using jobSeeker.DataAccess.Data.Repository.MessageRepo;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.SignalR;

namespace jobSeeker.SingleR
{
    public class MessageHub:Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public MessageHub(IMessageRepository messageRepository,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _messageRepository = messageRepository; 
            _userRepository = userRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();

            if (string.IsNullOrEmpty(otherUser))
            {
                throw new HubException("Invalid user specified.");
            }

            var groupName = GetGroupName(Context.User.Identity.Name, otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await _messageRepository
                .GetMessageThreadAsync(Context.User.Identity.Name, otherUser);

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
           
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<bool> SendMessage(CreateMessageDTO createMessageDTO)
        {
            var username = Context.User.Identity?.Name;

            if (username == createMessageDTO.RecipientUserName.ToLower())
                throw new HubException("You cannot send a message to yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDTO.RecipientUserName);

            if (recipient == null) throw new HubException("Recipient not found");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDTO.Content
            };

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveChangesAsync())
            {
                var groupName = GetGroupName(sender.UserName, recipient.UserName);
                var messageDTO = _mapper.Map<MessageDTO>(message);
                await Clients.Group(groupName).SendAsync("NewMessage", messageDTO);
                return true;
            }

            return false;
        }


        private string GetGroupName(string caller,string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

    }
}
