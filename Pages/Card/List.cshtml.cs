using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shoes_Shop.Models;

namespace Shoes_Shop.Pages.Card
{
    public class ListModel : PageModel
    {
		private readonly ShoesShopContext db;
		public ListModel(ShoesShopContext _db)
		{
			db = _db;
		}
		public int uid = 1;

		
		public List<Cart> Carts { get; set; }
		public void OnGet()
		{
			
			Carts = db.Carts.Include(c => c.Product).Where(c => c.UserId == 1)
					   .Select(c => new Cart
					   {
						   Product = c.Product,
						   Quantity = c.Quantity,
						   Color = c.Color,
						   Size = c.Size
					   }).ToList();
		}
	}
}
