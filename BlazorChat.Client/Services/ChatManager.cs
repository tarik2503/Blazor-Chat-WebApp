
using BlazorChat.Data;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace BlazorChat.Client.Services
{
    public class ChatManager : IChatManager
    {
       
        private readonly HttpClient _httpClient;
        public ChatManager(HttpClient httpClient)
        {
            _httpClient = httpClient; 
        }
        public async Task<List<MessageChat>> GetConversationAsync(string contactId, string Token)
        {
             
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",Token);

            return await _httpClient.GetFromJsonAsync<List<MessageChat>>($"api/chat/{contactId}");
        }
        public async Task<User> GetUserDetailsAsync(string userId, string Token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",Token);

            return await _httpClient.GetFromJsonAsync<User>($"api/chat/users/{userId}");
        }
        public async Task<List<User>> GetUsersAsync(string Token)
        {           
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",Token);

            var data = await _httpClient.GetFromJsonAsync<List<User>>("api/chat/users");
            return data;
        }
        public async Task SaveMessageAsync(MessageChat message, string Token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",Token);

            var rsult = await _httpClient.PostAsJsonAsync("api/chat", message);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
           

            return await _httpClient.GetFromJsonAsync<User>($"api/chat/user/{email}");
        }



    }
}
