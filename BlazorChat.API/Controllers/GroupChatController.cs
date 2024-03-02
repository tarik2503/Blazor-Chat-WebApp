using BlazorChat.API.Services;
using BlazorChat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupChatController : ControllerBase
    {
        public readonly IGroupChat _groupchatRepo;
        public readonly IUserRepository _userRepo;
        public GroupChatController(IUserRepository _userRepo, IGroupChat _groupchatRepo)
        {
            this._userRepo = _userRepo;
            this._groupchatRepo = _groupchatRepo;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateGroupAsync(Groups group)
        {
            var userId = User.Claims.Where(a => a.Type == "id").Select(a => a.Value).FirstOrDefault();
            bool save = await _groupchatRepo.CreateGroup(group, userId);
            if (save)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveGroupMessageAsync(GroupChat message)
        {
            var userId = User.Claims.Where(a => a.Type == "id").Select(a => a.Value).FirstOrDefault();
            bool save = await _groupchatRepo.SaveGroupMessageAsync(message, userId);
            if (save)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("{groupId:guid}")]
        public async Task<IActionResult> GetConversationAsync(Guid groupId)
        {
           
            var conversation = await _groupchatRepo.GetGroupConversationAsync(groupId);
            if (conversation == null)
            {
                return BadRequest();
            }

            return Ok(conversation);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetGroupsAsync()
        {
            
            var groups = await _groupchatRepo.GetGroupsAsync();
            if (groups == null)
            {
                return Ok("Create Group");
            }

            return Ok(groups);
        }

        [Authorize]
        [HttpGet("group/{groupId:guid}")]
        public async Task<IActionResult> GetUserDetailsAsync(Guid groupId)
        {
            var group = await _groupchatRepo.GetGroupDetailsAsync(groupId);
            if (group == null)
            {
                return BadRequest();
            }
            return Ok(group);
        }

    }
}
