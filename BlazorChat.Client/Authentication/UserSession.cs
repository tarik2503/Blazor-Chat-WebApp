using System.ComponentModel.DataAnnotations;

namespace BlazorChat.Client.Authentication
{
    public class UserSession
    {
        public string Name { get; set;}
        public string Role {  get; set;}
        [EmailAddress]
        public string UserEmail {  get; set;}

       
        public string UserIdentifier { get; set;}
    }
}
