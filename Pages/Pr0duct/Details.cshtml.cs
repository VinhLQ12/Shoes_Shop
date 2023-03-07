using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shoes_Shop.Models;
using System.Text.Json;

namespace Shoes_Shop.Pages.Shop
{
    public class DetailsModel : PageModel
    {
		private readonly ShoesShopContext db;
		public DetailsModel(ShoesShopContext _db)
		{
			db = _db;
		}


		public Product product { get; set; }

		public IActionResult OnGet(int ?id)
		{
			if (id == null)
			{
				return NotFound();
			}

			product = db.Products.Include(c=>c.Category).FirstOrDefault(m => m.Id == id);
			
	

			if (product == null)
			{
				return NotFound();
			}
			return Page();
		}

		

	}
}
