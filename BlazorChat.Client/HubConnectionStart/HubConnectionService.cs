using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorChat.Client.HubConnectionStart
{
    public class HubConnectionService
    {
        private readonly NavigationManager _navigationManager;
        public HubConnectionService(NavigationManager navigationManager)
        {

            _navigationManager = navigationManager;

        }
        public HubConnection hubConnection { get; set; }
        public async Task<HubConnection> StartConnection(string Token)
        {

             hubConnection = new HubConnectionBuilder().WithUrl(_navigationManager.ToAbsoluteUri("http://localhost:7203/signalRHub"), options =>

                  {
                      options.AccessTokenProvider = () => Task.FromResult(Token);

                      options.SkipNegotiation = true;

                      options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;

                  }).WithAutomaticReconnect().Build();

            await hubConnection.StartAsync();
            return hubConnection;
        }
    }

}
