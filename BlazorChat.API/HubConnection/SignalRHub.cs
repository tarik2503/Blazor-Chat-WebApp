
using BlazorChat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BlazorChat.API.HubConnection
{
    
    public class SignalRHub: Hub
    {
        

        private static HashSet<string> UserName = new HashSet<string>();        
        public override async Task OnConnectedAsync()
        {
                       
        }

        [Authorize]
        public async Task SendMessageAsync(MessageChat message, string userName)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, userName);
        }

        [Authorize]
        public async Task SendGroupMessageAsync(GroupChat message, string groupName)
        {
            await Clients.All.SendAsync("ReceiveGroupMessage", message, groupName);
        }
        public async Task ChatNotificationAsync(string message, string receiverUserId, string senderUserId)
        {
            await Clients.All.SendAsync("ReceiveChatNotification", message, receiverUserId, senderUserId);
        }

        public async Task GroupChatNotificationAsync(string message, string groupId, string senderUserId)
        {
            await Clients.All.SendAsync("ReceiveGroupChatNotification", message, groupId, senderUserId);
        }

        [Authorize]
        public async Task UserLogout()
        {
            var userName = Context.User.FindFirstValue("username");
            if (userName != null)
            {
                UserName.Remove(userName);
            }
            await Clients.All.SendAsync("UpdateOnlineUsers", UserName);
        }

        [Authorize]
        public async Task UserOnline()
        {
            var userName = Context.User.FindFirstValue("username");

            if (!UserName.Contains(userName) && userName != null)
            {
                UserName.Add(userName);
            }
            await Clients.All.SendAsync("GetOnlineUsers", UserName);
        }

        [AllowAnonymous]
        public async Task GetNewRegisteredUser(User user, string userEmail)
        {     
            await Clients.All.SendAsync("ReceiveNewRegisteredUser", user,userEmail);
        }
    }
}
