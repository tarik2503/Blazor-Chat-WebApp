using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorChat.Data.EmailModel
{
    public class Message
    {
        public MailboxAddress To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(string emailTo, string subject, string content)
        {
            To = new MailboxAddress("email", emailTo);
            Subject = subject;
            Content = content;
        }
    }
}
