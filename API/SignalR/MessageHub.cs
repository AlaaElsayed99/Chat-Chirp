using API.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;

namespace API.SignalR
{
    [Authorize]
    public class MessageHub:Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _precencehub;

        public MessageHub(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper, IHubContext<PresenceHub> precencehub)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _precencehub = precencehub;
            
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext= Context.GetHttpContext();

            var otherUser = httpContext.Request.Query["user"];
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId,groupName);
           var group=  await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdateGroup", group);

            var messages = await _messageRepository
                .GetMessageThread(Context.User.GetUsername(), otherUser);
            await Clients.Caller.SendAsync("ReceiveMessageThread",messages);
           
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
           var group= await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDTO createMessageDTO)
        {
            var username = Context.User.GetUsername();
            if (username == createMessageDTO.RecipientUsername.ToLower())
            {
                throw new HubException("You can't send message to yourself");
            }
            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDTO.RecipientUsername);
            if (recipient == null) throw new HubException("Not found User");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDTO.Content,

            };

            var groupName=GetGroupName(sender.UserName,recipient.UserName);

            var group = await _messageRepository.GetMessageGroup(groupName);


            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }

            else
            {
                var connections= await PresenceTracker.GetConnectionForUsers(recipient.UserName);
                if(connections != null)
                {
                    await _precencehub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new { username = sender.UserName, knownAs = sender.KnownAs });
                }
            }

            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync())
            {
                await Clients.Group(groupName).SendAsync("NewMessage",  _mapper.Map<MessageDTo>(message));
            }
            

        }

        private string GetGroupName(string caller, string other) 
        {
            var stringCompare = string.CompareOrdinal(caller,other)<0; 
            return stringCompare ? $"{caller}--{other}" : $"{other}--{caller}";

        }


        private async Task<Group> AddToGroup(string? groupName)
        {
            var group=await _messageRepository.GetMessageGroup(groupName);
            var connection = new connection(Context.ConnectionId, Context.User.GetUsername());
            if (group == null)
            {
                group = new Group(groupName);
                _messageRepository.AddGroup(group);

            }

            group.Connections.Add(connection);
            if( await _messageRepository.SaveAllAsync()) return group;
            throw new HubException("Failed to add to group");

        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _messageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x=>x.ConnectionId== Context.ConnectionId);
            _messageRepository.RemoveConnection(connection);
            if (await _messageRepository.SaveAllAsync()) return group;
            throw new HubException("Failed to removed from group");
        }
    }
}
