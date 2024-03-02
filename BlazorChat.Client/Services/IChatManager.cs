using BlazorChat.Data;

namespace BlazorChat.Client.Services
{
    public interface IChatManager
    {
        Task<List<User>> GetUsersAsync(string Token);
        Task SaveMessageAsync(MessageChat message, string Token);
        Task<List<MessageChat>> GetConversationAsync(string contactId, string Token);
        Task<User> GetUserDetailsAsync(string userId, string Token);
        Task<User> GetUserByEmailAsync(string email);
    }
}
