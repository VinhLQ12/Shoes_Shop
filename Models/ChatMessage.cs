using System;
using System.Collections.Generic;

namespace Shoes_Shop.Models
{
    public partial class ChatMessage
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; } = null!;
        public DateTime DateSent { get; set; }

        public virtual User Receiver { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }
}
