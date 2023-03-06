using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shoes_Shop.Models;

namespace Shoes_Shop.Pages.Shop
{
    public class IndexModel : PageModel
    {
		private readonly ShoesShopContext db;
		public IndexModel(ShoesShopContext _db)
		{
			db = _db;
		}

		public List<Category> Categories = new List<Category>();
		public List<Product> Products = new List<Product>();
		public void OnGet()
        {
			Categories = db.Categories.ToList();
			Products = db.Products.ToList();
		}
    }
}
