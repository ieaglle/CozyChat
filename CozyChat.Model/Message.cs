using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CozyChat.Model
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(ChatRoom))]
    [KnownType(typeof(User))]
    public class Message
    {
        [DataMember]
        public int Id { get; set; }
        [Required]
        [DataMember]
        public string Content { get; set; }
        [Required]
        [DataMember]
        public DateTime SentDate { get; set; }
        [Required]
        [DataMember]
        public bool IsRead { get; set; }
        [DataMember]
        public int? ChatRoomId { get; set; }
        [DataMember]
        [ForeignKey("Receiver")]
        public int? ReceiverId { get; set; }
        [DataMember]
        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        [DataMember]
        public ChatRoom ChatRoom { get; set; }
        [DataMember]
        [InverseProperty("ReceivedMessages")]
        public User Receiver { get; set; }
        [DataMember]
        [InverseProperty("SentMessages")]
        public User Sender { get; set; }
    }
}