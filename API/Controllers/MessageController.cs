using API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    public class MessageController : ControllerBase
    {

        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public MessageController(IMessageRepository messageRepository, IMapper mapper, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        [HttpPost]
        public async Task<ActionResult<MessageDTo>> CreateMessage(CreateMessageDTO createMessageDTO)
        {
            var username = User.GetUsername();
            if (username == null) return NotFound("Not Found");
            if (username == createMessageDTO.RecipientUsername.ToLower())
            {
                return BadRequest("you cannot send message to yourself");
            }
            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDTO.RecipientUsername);
            if (recipient == null) return NotFound("No Recipient");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDTO.Content,

            };

            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDTo>(message));
            return BadRequest("failed to send message");


        }
        [HttpGet]
        public async Task<ActionResult<PageList<MessageDTo>>> GetMessageForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await _messageRepository.GetMessageForUser(messageParams);
            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage,
                messages.PageSize, messages.TotalCount, messages.TotalPages));
            return messages;
        }
        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTo>>> getMessageThread(string username)
        {
            var currentUsername = User.GetUsername();
            return Ok(await _messageRepository.GetMessageThread(currentUsername, username));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username=User.GetUsername();
            var deletedmessage =await _messageRepository.GetMessageAsync(id);
            if (deletedmessage.SenderUsername != username&& deletedmessage.RecipientUsername!= username)
                return Unauthorized();
            if (deletedmessage.SenderUsername == username)
                deletedmessage.SenderDeleted = true;
            if(deletedmessage.RecipientUsername==username)
                deletedmessage.RecipientDeleted= true;
            if(deletedmessage.SenderDeleted&& deletedmessage.RecipientDeleted)
                _messageRepository.DeleteMessage(deletedmessage);
            if( await _messageRepository.SaveAllAsync()) return Ok();
            return BadRequest("problem to delete message");
        }



    }
}
