using BlazorChat.Data.ModelDTO;

namespace BlazorChat.Client.Services
{
    public interface IAccountManager
    {
        Task<HttpResponseMessage> SignupUser(UserSignupDto UserDto);
        Task<HttpResponseMessage> LoginUser(UserLoginDto loginDto);
    }
}
