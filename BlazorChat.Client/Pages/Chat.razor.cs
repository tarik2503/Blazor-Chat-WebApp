using BlazorChat.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace BlazorChat.Client.Pages
{
    public partial class Chat
    {
        [CascadingParameter] public HubConnection hubConnection { get; set; }
        [Parameter] public string CurrentMessage { get; set; }
        [Parameter] public string CurrentUserId { get; set; }
        [Parameter] public string CurrentUserEmail { get; set; }
        private List<MessageChat> messages = new List<MessageChat>();
        

        public string Token { get; set; }
        private async Task SubmitAsync()
        {
            if (!string.IsNullOrEmpty(CurrentMessage) && !string.IsNullOrEmpty(ContactId))
            {
                var chatHistory = new MessageChat()
                {
                    Message = CurrentMessage,
                    ToUserId = ContactId,
                    CreatedDate = DateTime.Now
                };
                chatHistory.FromUserId = CurrentUserId;
                await _chatManager.SaveMessageAsync(chatHistory, Token);
                await hubConnection.SendAsync("SendMessageAsync", chatHistory, CurrentUserEmail);
                CurrentMessage = string.Empty;
            }
        }
       
        protected override async Task OnInitializedAsync()
        {
            Token = await session.GetToken();
            var state = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = state.User;

            if (hubConnection == null)
            {
                hubConnection = await _hubConnection.StartConnection(Token);
            }
            hubConnection.On<MessageChat, string>("ReceiveMessage", async (message, userName) =>
                {
                    if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId) || (ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
                    {

                        if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId))
                        {
                            messages.Add(new MessageChat { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new User() { Email = CurrentUserEmail, FirstName = user.Claims.Where(a => a.Type == "name").Select(a => a.Value).FirstOrDefault(), Id = CurrentUserId } });
                            await hubConnection.SendAsync("ChatNotificationAsync", $"New Message From {userName}", ContactId, CurrentUserId);
                        }
                        else if ((ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
                        {
                            messages.Add(new MessageChat { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new User() { Email = ContactEmail, FirstName = ContactName, Id = ContactId } });
                        }
                        await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
                        StateHasChanged();
                    }
                });

                hubConnection.On<User, string>("ReceiveNewRegisteredUser", async (user, userEmail) =>
                {
                    if (userEmail != CurrentUserEmail)
                    {
                        ChatUsers.Add(user);
                    }
                    StateHasChanged();
                });
            
            await GetUsersAsync();

            hubConnection.On<HashSet<string>>("GetOnlineUsers", async (userName) =>
             {
                 if (userName != null)
                 {
                     onlineUsers = userName;
                     foreach (var user in ChatUsers)
                     {
                         if (onlineUsers.Contains(user.UserName))
                         {
                             user.Status = true;
                         }
                     }
                 }
                 StateHasChanged();
             });


            hubConnection.On<HashSet<string>>("UpdateOnlineUsers", async (userName) =>
            {
                if (userName != null)
                {
                    onlineUsers = userName;
                    foreach (var user in ChatUsers)
                    {
                        if (!onlineUsers.Contains(user.UserName))
                        {
                            user.Status = false;
                        }
                        
                    }
                }
                StateHasChanged();
            });

            CurrentUserId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            CurrentUserEmail = user.Claims.Where(a => a.Type == ClaimTypes.Email).Select(a => a.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(ContactId))
            {
                await LoadUserChat(ContactId);
            }

            await hubConnection.SendAsync("UserOnline");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");

   
        }

        public List<User> ChatUsers = new List<User>();
        public HashSet<string> onlineUsers = new HashSet<string>();
        [Parameter] public string ContactName { get; set; }
        [Parameter] public string ContactEmail { get; set; }
        [Parameter] public string ContactId { get; set; }
        async Task LoadUserChat(string userId)
        {
            var contact = await _chatManager.GetUserDetailsAsync(userId, Token);
            ContactId = contact.Id;
            ContactName = contact.FirstName;
            ContactEmail = contact.Email;
            _navigationManager.NavigateTo($"chat/{ContactId}");
            messages = new List<MessageChat>();
            messages = await _chatManager.GetConversationAsync(ContactId, Token);
        }

        private async Task GetUsersAsync()
        {     
            ChatUsers = await _chatManager.GetUsersAsync(Token);            
        }

        private async Task LoadGroupChat()
        {
            _navigationManager.NavigateTo($"/groupschat");
        }
       
      
        
    }
}

