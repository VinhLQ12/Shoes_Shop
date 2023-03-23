using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shoes_Shop.Models;

namespace Shoes_Shop.Pages {
    public class IndexModel : PageModel {
		private readonly ShoesShopContext db;
		public IndexModel(ShoesShopContext _db)
		{
			db = _db;
		}

		public int uid = 1;

		public List<Category> Categories { get; set; }
		[BindProperty]
		public List<Product> NewProducts { get; set; }
		[BindProperty]
		public List<Product> BestSaleProducts { get; set; }
		public List<Cart> Carts { get; set; }
		public double totalPrice { get; set; }
        public int CartCount { get; set; }
        public void OnGet()
		{

            CartCount =  db.Carts
               .Where(c => c.UserId == 1)
               .Select(c => c.Id)
               .Distinct()
               .Count();
            

        
        var products = db.Products.ToList();

			NewProducts = db.Products
				  .OrderByDescending(p => p.Id).Take(6).ToList();


			var idsBestSale = db.OrderDetails
				   .GroupBy(d => d.ProductId)
				   .Select(g => new { ProductId = g.Key, Sum = g.Sum(d => d.Quantity) })
				   .OrderByDescending(o => o.Sum)
				   .Take(4);

			BestSaleProducts = new List<Product>();
			foreach (var id in idsBestSale)
			{
				BestSaleProducts.Add(products.First(p => p.Id == id.ProductId));
			}


			Carts = db.Carts.Include(c => c.Product).Where(c => c.UserId == 1)
					   .Select(c => new Cart
					   {
						   Product = c.Product,
						   Quantity = c.Quantity
					   }).ToList();

		

			totalPrice = 0;
			foreach (var cart in Carts)
			{
				totalPrice += (double)cart.Product.Price * cart.Quantity;
			}
		}
	}
}