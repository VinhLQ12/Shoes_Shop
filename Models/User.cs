using System;
using System.Collections.Generic;

namespace Shoes_Shop.Models
{
    public partial class User
    {
        public User()
        {
            ChatMessageReceivers = new HashSet<ChatMessage>();
            ChatMessageSenders = new HashSet<ChatMessage>();
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int IsSeller { get; set; }

        public virtual ICollection<ChatMessage> ChatMessageReceivers { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageSenders { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
