using BlazorChat.Data;

namespace BlazorChat.API.Services
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(string userId);
        Task<User> GetUserDetailsAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
    }
}
