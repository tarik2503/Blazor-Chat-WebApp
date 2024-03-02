using BlazorChat.Data;
using BlazorChat.Data.ModelDTO;

namespace BlazorChat.API.Services
{
    public interface IChatRepository
    {
       
        Task<bool> SaveMessageAsync(MessageChat message, string userId);
        Task<IEnumerable<MessageChat>> GetConversationAsync(string contactId, string userId);
    }
}
