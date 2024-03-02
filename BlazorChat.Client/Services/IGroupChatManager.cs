using BlazorChat.Data;

namespace BlazorChat.Client.Services
{
    public interface IGroupChatManager
    {
        Task SaveGroupMessageAsync(GroupChat message, string Token);
        Task<List<GroupChat>> GetGroupConversationAsync(Guid groupId, string Token);
        Task CreateGroup(Groups group, string Token);
        Task<List<Groups>> GetGroupsAsync(string Token);

        Task<Groups> GetGroupDetailsAsync(Guid groupId, string Token);
    }
}
