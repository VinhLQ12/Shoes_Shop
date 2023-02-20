using System;
using System.Collections.Generic;

namespace Shoes_Shop.Models
{
    public partial class Sale
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int SalePrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
