using AutoMapper;
using jobSeeker.DataAccess.Data.Repository.IUserRepository;
using jobSeeker.DataAccess.Data.Repository.MessageRepo;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jobSeeker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageController(IUserRepository userRepository,
            IMessageRepository messageRepository,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO createmessageDTO)
        {
            var username = User?.Identity?.Name;

            if (username == createmessageDTO.RecipientUserName.ToLower())
                return BadRequest("you cannot send message to yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createmessageDTO.RecipientUserName);

            if (recipient == null) return NotFound();
            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createmessageDTO.Content
            };
            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveChangesAsync()) return Ok(_mapper.Map<MessageDTO>(message));

            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User?.Identity?.Name;
            if (string.IsNullOrEmpty(messageParams.Username))
            {
                return Unauthorized("User is not authenticated.");
            }
            var messages = await _messageRepository.GetMessagesForUserAsync(messageParams);
            return Ok(messages);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThread(string username)
        {
            var currentUsername=User.Identity?.Name;

            return Ok(await _messageRepository.GetMessageThreadAsync(currentUsername, username));
        }

        [HttpGet("chatted-users")]
        public async Task<ActionResult<IEnumerable<MessageUserDTO>>> GetChattedUsers()
        {
            var username = User?.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return Unauthorized("User is not authenticated.");

            var chattedUsers = await _messageRepository.GetChattedUsersAsync(username);

            return Ok(chattedUsers);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username= User?.Identity?.Name;
            var message = await _messageRepository.GetMessage(id);
            if(message.Sender.UserName!= username&& message.Recipient.UserName!= username)
            {
                return Unauthorized();
            }

            if(message.Sender.UserName== username)
            {
                message.SenderDeleted = true;
            }

            if(message.Recipient.UserName== username)
            {
                message.RecipientDeleted = true;
            }

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _messageRepository.DeleteMessage(message);
            }

            if (await _messageRepository.SaveChangesAsync()) return Ok();



            return BadRequest("Problem deleting the message");
        }

        


    }
}
