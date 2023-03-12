using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Shoes_Shop.Models;

namespace Shoes_Shop.Pages.Shop {
    public class IndexModel : PageModel {
        private readonly ShoesShopContext db;
        public IndexModel(ShoesShopContext _db)
        {
            db = _db;
        }

        public int uid = 1;

        public List<Category> Categories = new List<Category>();
        public List<Product> Products = new List<Product>();
        public List<Cart> Carts { get; set; }

        public void OnGet()
        {
            Categories = db.Categories.ToList();
            Products = db.Products.ToList();

            Carts = db.Carts.Include(c => c.Product).Where(c => c.UserId == 1)
                       .Select(c => new Cart
                       {
                           Product = c.Product,
                           Quantity = c.Quantity
                       }).ToList();
        }

    }

}
