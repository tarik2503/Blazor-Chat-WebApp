using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorChat.Data
{
    public class GroupChat
    {
        public long Id { get; set; }
        public Guid GroupId { get; set; }
        public string SenderId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Groups? Group { get; set; }
        public virtual User? Sender { get; set; }
    }
}
