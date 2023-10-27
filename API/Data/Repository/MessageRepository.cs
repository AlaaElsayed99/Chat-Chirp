using AutoMapper.QueryableExtensions;

namespace API.Data.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(AppDbContext context , IMapper mapper)
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

        public async Task<Message> GetMessageAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PageList<MessageDTo>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .OrderByDescending(x => x.MessageSent)
                .AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(s => s.RecipientUsername == messageParams.Username &&s.RecipientDeleted==false ),
                "Outbox" => query.Where(s => s.SenderUsername == messageParams.Username&&s.SenderDeleted==false),
                _ => query.Where(u => u.RecipientUsername == messageParams.Username &&
                    u.RecipientDeleted==false && u.DateRead == null)
            };
            var messages = query.ProjectTo<MessageDTo>(_mapper.ConfigurationProvider);
            return await PageList<MessageDTo>
                .CreateAsync(messages,messageParams.PageNumber,messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDTo>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _context.Messages
                .Include(s => s.Sender).ThenInclude(s => s.Photos)
                .Include(s => s.Recipient).ThenInclude(p => p.Photos)
                .Where(
                    m => m.RecipientUsername == currentUsername && m.RecipientDeleted == false && 
                    m.SenderUsername == recipientUsername ||
                    m.RecipientUsername == recipientUsername && m.RecipientDeleted == false &&
                    m.SenderUsername == currentUsername
                ).OrderBy(m => m.MessageSent).ToListAsync();
            var unreadMessage = messages.Where(m => m.DateRead == null &&
                 m.RecipientUsername == currentUsername).ToList();
            if(unreadMessage.Any())
            {
                foreach (var message in messages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();

            }
            return _mapper.Map<IEnumerable<MessageDTo>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
