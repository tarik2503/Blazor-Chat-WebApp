using BlazorChat.Data.ModelDTO;

namespace BlazorChat.API.Services
{
    public interface IAccountRepository
    {
        Task<bool> CheckUser(string Email);
        Task<bool> SignupUser(UserSignupDto UserDto);
        Task<bool> LoginUser(UserLoginDto loginDto);
        Task<string> CreateToken(string Email);



    }
}
