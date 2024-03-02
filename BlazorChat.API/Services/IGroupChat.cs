using BlazorChat.Data;

namespace BlazorChat.API.Services
{
    public interface IGroupChat
    {
        Task<bool> SaveGroupMessageAsync(GroupChat message, string userId);
        Task<IEnumerable<GroupChat>> GetGroupConversationAsync(Guid groupId);
        Task<bool> CreateGroup(Groups group, string userId);
        Task<IEnumerable<Groups>> GetGroupsAsync();
        Task<Groups> GetGroupDetailsAsync(Guid groupId);
    }
}
