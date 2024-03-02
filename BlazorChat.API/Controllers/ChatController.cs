using BlazorChat.API.Services;
using BlazorChat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorChat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ChatController : ControllerBase
    {
        public readonly IChatRepository _chatRepo;
        public readonly IUserRepository _userRepo;
        public ChatController(IUserRepository _userRepo,IChatRepository _chatRepo)
        {
            this._userRepo = _userRepo;
            this._chatRepo = _chatRepo;
        }
        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var userId = User.Claims.Where(a => a.Type == "id").Select(a => a.Value).FirstOrDefault();
          
            var userList = await _userRepo.GetUsersAsync(userId);
            
            
            if(userList == null)
            {
                return NotFound();
            }
            return Ok(userList);
            
        }
        [Authorize]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserDetailsAsync(string userId)
        {
            var user =await _userRepo.GetUserDetailsAsync(userId);
            if(user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveMessageAsync(MessageChat message)
        {
            var userId = User.Claims.Where(a => a.Type == "id").Select(a => a.Value).FirstOrDefault();
            bool save =await _chatRepo.SaveMessageAsync(message, userId);
            if(save)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetConversationAsync(string contactId)
        {
            var userId = User.Claims.Where(a => a.Type == "id").Select(a => a.Value).FirstOrDefault();
            var conversation =await _chatRepo.GetConversationAsync(contactId, userId);
            if(conversation == null)
            {
                return BadRequest();
            }

            return Ok(conversation);
        }

        [AllowAnonymous]
        [HttpGet("user/{email}")]
        public async Task<IActionResult> GetUserDetailsByEmailAsync(string email)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
    }
}
