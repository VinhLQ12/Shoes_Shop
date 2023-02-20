using System;
using System.Collections.Generic;

namespace Shoes_Shop.Models
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public virtual Color Color { get; set; } = null!;
        public virtual Size Size { get; set; } = null!;
    }
}
