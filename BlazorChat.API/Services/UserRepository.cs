using BlazorChat.Data.DBContext;
using BlazorChat.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorChat.API.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly BlazorChatDBContext _context;

        public UserRepository(BlazorChatDBContext _context, UserManager<User> _userManager)
        {
            this._context = _context;
            this._userManager = _userManager;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string userId)
        {
            var allUsers = await _context.Users.Where(user => user.Id != userId).ToListAsync();
            return allUsers;

        }

        public async Task<User> GetUserDetailsAsync(string userId)
        {
            var user = await _context.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
            return user;
        }
    }
}
