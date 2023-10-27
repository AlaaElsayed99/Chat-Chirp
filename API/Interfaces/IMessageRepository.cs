using Newtonsoft.Json.Linq;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessageAsync(int id);
        Task<PageList<MessageDTo>> GetMessageForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDTo>> GetMessageThread(string currentUsername, string recipientUsername);
        Task<bool> SaveAllAsync();



    }
}
