using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CozyChat.Model
{
    [DataContract(IsReference = true)]
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime SentDate { get; set; }
        [Required]
        public bool IsRead { get; set; }
        public int? ChatRoomId { get; set; }
        [ForeignKey("Receiver")]
        public int? ReceiverId { get; set; }
        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        public ChatRoom ChatRoom { get; set; }
        [InverseProperty("ReceivedMessages")]
        public User Receiver { get; set; }
        [InverseProperty("SentMessages")]
        public User Sender { get; set; }
    }
}