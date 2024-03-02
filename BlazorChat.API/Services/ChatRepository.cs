using BlazorChat.Data;
using BlazorChat.Data.DBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace BlazorChat.API.Services
{
    public class ChatRepository:IChatRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly BlazorChatDBContext _context;

        public ChatRepository(BlazorChatDBContext _context, UserManager<User> _userManager)
        {
            this._context = _context;
            this._userManager = _userManager;
        }

       

        public async Task<bool> SaveMessageAsync(MessageChat message, string userId)
        {
            message.FromUserId = userId;
            message.CreatedDate = DateTime.Now;
            message.ToUser = await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync();
           
            if(message.ToUser != null || userId != null)
            {
                await _context.MessageChats.AddAsync(message);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
            
        }

        public async Task<IEnumerable<MessageChat>> GetConversationAsync(string contactId,string userId)
        {
           
                var messages = await _context.MessageChats
                        .Where(h => (h.FromUserId == contactId && h.ToUserId == userId) || (h.FromUserId == userId && h.ToUserId == contactId))
                        .OrderBy(a => a.CreatedDate)
                        .Include(a => a.FromUser)
                        .Include(a => a.ToUser)
                        .Select(x => new MessageChat
                        {
                            FromUserId = x.FromUserId,
                            Message = x.Message,
                            CreatedDate = x.CreatedDate,
                            Id = x.Id,
                            ToUserId = x.ToUserId,
                            ToUser = x.ToUser,
                            FromUser = x.FromUser
                        }).ToListAsync();
                
                return messages;
        }
    }
}
