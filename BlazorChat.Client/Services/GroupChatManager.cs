using BlazorChat.Data;
using BlazorChat.Data.EmailModel;
using System.Net.Http.Headers;

namespace BlazorChat.Client.Services
{
    public class GroupChatManager : IGroupChatManager
    {
        private readonly HttpClient _httpClient;
        public GroupChatManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateGroup(Groups group, string Token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
           var res = await _httpClient.PostAsJsonAsync("api/groupchat/create", group);
        }

        public async Task<List<GroupChat>> GetGroupConversationAsync(Guid groupId, string Token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            
            var conversation = await _httpClient.GetFromJsonAsync<List<GroupChat>>($"api/groupchat/{groupId}");
            return conversation;
        }

        public async Task<List<Groups>> GetGroupsAsync(string Token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var groups = await _httpClient.GetFromJsonAsync<List<Groups>>("api/groupchat");
            return groups;
        }

        public async Task SaveGroupMessageAsync(GroupChat message, string Token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

             await _httpClient.PostAsJsonAsync("api/groupchat", message);
        }

        public async Task<Groups> GetGroupDetailsAsync(Guid groupId, string Token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            

            return await _httpClient.GetFromJsonAsync<Groups>($"api/groupchat/group/{groupId}");
        }
    }
}
