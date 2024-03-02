using BlazorChat.Data;
using BlazorChat.Data.ModelDTO;

namespace BlazorChat.Client.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly HttpClient _httpClient;
        public AccountManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> SignupUser(UserSignupDto userDto)
        {
           return await _httpClient.PostAsJsonAsync("api/account", userDto);
           
        }

        public async Task<HttpResponseMessage> LoginUser(UserLoginDto userDto)
        {
           return  await _httpClient.PostAsJsonAsync("api/account/login", userDto);

        }
    }
}
