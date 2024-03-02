namespace BlazorChat.Client.Services
{
    using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

    public class Session
    {
        public readonly ProtectedSessionStorage ProtectedSessionStorage;

        public Session(ProtectedSessionStorage _ProtectedSessionStore)
        {
            ProtectedSessionStorage = _ProtectedSessionStore;
        }

        public async Task<String> GetToken()
        {
            var val = await ProtectedSessionStorage.GetAsync<String>("UserToken");
            return val.Value;
        }
    }     
}
