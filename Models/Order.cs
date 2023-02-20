using System;
using System.Collections.Generic;

namespace Shoes_Shop.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateOrdered { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string DeliveryLocation { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
