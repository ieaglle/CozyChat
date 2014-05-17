using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CozyChat.Model
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(Message))]
    [KnownType(typeof(User))]
    public class ChatRoom
    {
        public ChatRoom()
        {
            this.Messages = new HashSet<Message>();
            this.Users = new HashSet<User>();
        }

        [DataMember]
        public int Id { get; set; }
        [Required, StringLength(50)]
        [DataMember]
        public string Name { get; set; }
        [Required]
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [Required]
        [DataMember]
        public bool IsCurrent { get; set; }
        [Required]
        [DataMember]
        public int CreatorId { get; set; }

        [DataMember]
        public User Creator { get; set; }
        [DataMember]
        public ICollection<Message> Messages { get; set; }
        [DataMember]
        public ICollection<User> Users { get; set; } 
    }
}