using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorChat.Data
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Status {  get; set; }   
        public virtual ICollection<MessageChat> MessageChatsFromUsers { get; set; }
        public virtual ICollection<MessageChat> MessageChatsToUsers { get; set; }
        public virtual ICollection<GroupChat> GroupChatsFromUsers { get; set; }
        public virtual ICollection<Groups> GroupsByUser { get; set; }
        public User()
        {
            MessageChatsFromUsers = new HashSet<MessageChat>();
            MessageChatsToUsers = new HashSet<MessageChat>();
            GroupChatsFromUsers = new HashSet<GroupChat>();
            GroupsByUser = new HashSet<Groups>();
        }
    }
}
