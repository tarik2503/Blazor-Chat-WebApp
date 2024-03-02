using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorChat.Data
{
    public class Groups
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public virtual User? CreateGroup { get; set; }
        public virtual ICollection<GroupChat> GroupMessages { get; set; }

        public Groups()
        {
           
            GroupMessages = new HashSet<GroupChat>();
        }
    }
}
