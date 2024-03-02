using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorChat.Data.ModelDTO
{
    public class MessageChatDto
    {
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Message { get; set; }
    }
}
