using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CozyChat.Model
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(ChatRoom))]
    [KnownType(typeof(Message))]
    public class User
    {
        public User()
        {
            this.ChatRooms = new HashSet<ChatRoom>();
            this.ReceivedMessages = new HashSet<Message>();
            this.SentMessages = new HashSet<Message>();
        }
        [DataMember]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [DataMember]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        [DataMember]
        public string Password { get; set; }
        [Required]
        [DataMember]
        public DateTime RegisteredDate { get; set; }
        [Required]
        [DataMember]
        public DateTime LastSeenDate { get; set; }

        [DataMember]
        public ICollection<ChatRoom> ChatRooms { get; set; }
        [DataMember]
        public ICollection<Message> ReceivedMessages { get; set; }
        [DataMember]
        public ICollection<Message> SentMessages { get; set; }
    }
}