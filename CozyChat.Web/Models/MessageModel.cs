using System;

namespace CozyChat.Web.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Sent { get; set; }
        public string Sender { get; set; }
    }
}