using BlazorChat.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System.Security.Claims;

namespace BlazorChat.Client.Pages
{
    public partial class GroupsChat
    {

        [CascadingParameter] public HubConnection hubConnection { get; set; }
        [Parameter] public string CurrentMessage { get; set; }
        [Parameter] public string CurrentUserId { get; set; }
        [Parameter] public string CurrentUserEmail { get; set; }

        private List<GroupChat> messages = new List<GroupChat>();
        public string Token { get; set; }
        private async Task SubmitAsync()
        {
            if (!string.IsNullOrEmpty(CurrentMessage) && !string.IsNullOrEmpty(GroupId.ToString()))
            {
                var chatHistory = new GroupChat()
                {
                    Message = CurrentMessage,
                    GroupId = GroupId,
                    CreatedDate = DateTime.Now,
                     
                };
                chatHistory.SenderId = CurrentUserId;
                
                await _groupchatManager.SaveGroupMessageAsync(chatHistory, Token);
                await hubConnection.SendAsync("SendGroupMessageAsync", chatHistory, GroupName);
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
            hubConnection.On<GroupChat, string>("ReceiveGroupMessage", async (message, groupName) =>
            {
                if (GroupId == message.GroupId )
                {

                       
                    if ((GroupId == message.GroupId && CurrentUserId == message.SenderId))
                    {
                        messages.Add(new GroupChat { Message = message.Message, CreatedDate = message.CreatedDate, Sender = new User() { Email = CurrentUserEmail, FirstName = user.Claims.Where(a => a.Type == "name").Select(a => a.Value).FirstOrDefault(), Id = CurrentUserId } });
                        await hubConnection.SendAsync("GroupChatNotificationAsync", $"New Message in {groupName}", GroupId, CurrentUserId);
                    }
                    else if ((GroupId == message.GroupId && CurrentUserId != message.SenderId))
                    {
                        var sender = await _chatManager.GetUserDetailsAsync(message.SenderId, Token);
                        messages.Add(new GroupChat { Message = message.Message, CreatedDate = message.CreatedDate, Sender = new User() { Email = sender.Email, FirstName = sender.FirstName, Id = message.SenderId } });
                    }

                }
                await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
                StateHasChanged();
            });
            await GetGroupsAsync();
            CurrentUserId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            CurrentUserEmail = user.Claims.Where(a => a.Type == ClaimTypes.Email).Select(a => a.Value).FirstOrDefault();
            
            if ( GroupId != Guid.Empty)
            {
                await LoadGroupChat(GroupId);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
        }

       
        public List<Groups> ChatGroups = new List<Groups>();
        [Parameter] public Guid GroupId { get; set; }
        [Parameter] public string GroupName { get; set; }
        async Task LoadGroupChat(Guid groupId)
        {
            var group = await _groupchatManager.GetGroupDetailsAsync(groupId, Token);
            GroupId = group.Id;
            GroupName = group.Name;
            _navigationManager.NavigateTo($"groupschat/{GroupId}");
            messages = new List<GroupChat>();
            messages = await _groupchatManager.GetGroupConversationAsync(GroupId, Token);
        }

        private async Task GetGroupsAsync()
        {

            ChatGroups = await _groupchatManager.GetGroupsAsync(Token);

        }

        private async Task LoadUsersChat()
        {
            _navigationManager.NavigateTo($"/chat");
        }

        private async Task CreateGroup()
        {
            var parameters = new DialogParameters();
            parameters.Add("model", new Groups());
            var dialog = await dialogService.Show<AddGroup>("Create A Group", parameters).Result;
            
            if (dialog.Data != null)
            {
    
                    var group = new Groups()
                    {
                        Name = (dialog.Data as Groups).Name,
                        CreatedBy = CurrentUserId,

                    };
                await _groupchatManager.CreateGroup(group, Token);
                ChatGroups = await _groupchatManager.GetGroupsAsync(Token);
                StateHasChanged();

            }
        }

    }
}
