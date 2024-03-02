using BlazorChat.Data;
using BlazorChat.Data.DBContext;
using BlazorChat.Data.EmailModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BlazorChat.API.Services
{
    public class GroupChatRepository: IGroupChat
    {
        private readonly UserManager<User> _userManager;
        private readonly BlazorChatDBContext _context;

        public GroupChatRepository(BlazorChatDBContext _context, UserManager<User> _userManager)
        {
            this._context = _context;
            this._userManager = _userManager;
        }
        public async Task<bool> CreateGroup(Groups group, string userId)
        {
            group.CreatedBy = userId;
            group.CreateGroup = await _context.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
            if(group.CreateGroup != null)
            {
                await _context.UserGroups.AddAsync(group);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> SaveGroupMessageAsync(GroupChat message, string userId)
        {
            message.SenderId = userId;
            message.CreatedDate = DateTime.Now;
            message.Group = await _context.UserGroups.Where(group => group.Id == message.GroupId).FirstOrDefaultAsync();

            if (message.Group != null || userId != null)
            {
                await _context.GroupChats.AddAsync(message);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<IEnumerable<GroupChat>> GetGroupConversationAsync(Guid groupId)
        {

            var messages = await _context.GroupChats
                    .Where(h =>  h.GroupId == groupId )
                    .OrderBy(a => a.CreatedDate)
                    .Include(a => a.Sender)
                    .Include(a => a.Group)
                    .Select(x => new GroupChat
                    {
                        SenderId = x.SenderId,
                        Message = x.Message,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        GroupId = x.GroupId,
                        Group = x.Group,
                        Sender = x.Sender
                    }).ToListAsync();

            return messages;
        }

        public async Task<IEnumerable<Groups>> GetGroupsAsync()
        {
            var allGroups = await _context.UserGroups.ToListAsync();
            return allGroups;

        }

        public async Task<Groups> GetGroupDetailsAsync(Guid groupId)
        {
            var group = await _context.UserGroups.Where(group => group.Id == groupId).FirstOrDefaultAsync();
            return group;
        }
    }
}
