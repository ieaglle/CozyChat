// Oleksandr Babii
// 17/05/2014 11:38 
// 

using System;
using System.Collections.Generic;

namespace CozyChat.Web.Models
{
    public class ChatRoomModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public int CreatorId { get; set; }
        public string Creator { get; set; }

        public IEnumerable<int> Users { get; set; }
    }
}