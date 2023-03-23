using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shoes_Shop.Models;



namespace Shoes_Shop.Pages.Shop {
    public class DetailsModel : PageModel {

        private readonly ShoesShopContext db;


        public DetailsModel(ShoesShopContext _db)
        {
            db = _db;


        }

        public Product Product { get; set; }
        [BindProperty]
		public List<Cart> Carts { get; set; }
		public double totalPrice { get; set; }

		public IActionResult OnGet(int? id)
        {
	
			
			if (id == null)
            {
                return NotFound();
            }

            Product = db.Products
                .Include(p => p.Category)
                .Include(p => p.Inventories)
                    .ThenInclude(i => i.Color)
                .FirstOrDefault(p => p.Id == id);


            if (Product == null)
            {
                return NotFound();
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


			return Page();
        }


        public JsonResult OnGetGetSizeByColor(int productId, int colorId)
        {
            var sizes = db.Inventories
                .Include(i => i.Size)
                .Where(i => i.ColorId == colorId && i.ProductId == productId)
                .Select(i => new { sizeId = i.Size.SizeId, sizeName = i.Size.SizeName })
                .ToList();

            return new JsonResult(sizes);
        }

        public IActionResult OnPostAddToCart(int productId, int colorId, int sizeId, int quantity)
        {
            var product = db.Products
                .Include(p => p.Inventories)
                .FirstOrDefault(p => p.Id == productId);

            var inventory = product.Inventories.FirstOrDefault(i => i.ColorId == colorId && i.SizeId == sizeId);
            if (inventory == null || inventory.Quantity < quantity)
            {
                ViewData["Message"] = "Sorry, the selected color and size are out of stock.";
                return RedirectToPage("./Index");
            }
            Cart existingItem = db.Carts.Where(u => u.UserId == 1).FirstOrDefault(ci => ci.ProductId == productId && ci.ColorId == colorId && ci.SizeId == sizeId);

            if (existingItem != null)
            {
                // Product is already in the cart, update the quantity
                existingItem.Quantity += quantity;
                db.Carts.Update(existingItem);
                db.SaveChanges();
            }
            else
            {
                // Create cart item and add to cart
                var cartItem = new Cart
                {
                    UserId = 1,
                    ProductId = productId,
                    ColorId = colorId,
                    SizeId = sizeId,
                    Quantity = quantity,
                    DateAdded = DateTime.Now

                };

                // Get user's cart or create a new cart if it doesn't exist


                // Add cart item to cart
                db.Carts.Add(cartItem);

                // Save changes to database
                db.SaveChanges();
            }


            // Redirect user to cart page
            return RedirectToPage("/Card/List");
        }

    }
}

