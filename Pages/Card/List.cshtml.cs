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
		public double totalPrice { get; set; }
		public int CartCount { get; set; }
		public async Task<IActionResult> OnGet()
		{
            
            UpdatePrice();
			return Page();

		}

		public void UpdatePrice()
		{
			Carts = db.Carts.Include(c => c.Product).Where(c => c.UserId == 1)
					   .Select(c => new Cart
					   {
						   Product = c.Product,
						   Quantity = c.Quantity,
						   Color = c.Color,
						   Size = c.Size
					   }).ToList();

			totalPrice = 0;
			foreach (var cart in Carts)
			{
				totalPrice += (double)cart.Product.Price * cart.Quantity;
			}
		}

        
        [HttpPost]
		public async Task<IActionResult> OnPostUpdateQuantity([FromBody] int productId, [FromBody] int quantity)
		{
			{
				var cart = await db.Carts.FirstOrDefaultAsync(c => c.UserId == 1 && c.ProductId == productId);

				if (cart != null)
				{
					cart.Quantity = quantity;
					await db.SaveChangesAsync();

					UpdatePrice();

					return new JsonResult(new { success = true, message = "Cart updated successfully." });
				}

				return new JsonResult(new { success = false, message = "Cart not found." });
			}

		}


		public async Task<IActionResult> OnGetRemove(int id)
		{
			var cart = await db.Carts.FirstOrDefaultAsync(c => c.UserId == 1 && c.ProductId == id);
			if (cart != null)
			{
				db.Carts.Remove(cart);
				await db.SaveChangesAsync();
			}

			UpdatePrice();

			return RedirectToPage("./List");
		}



		public void UpdateInventory(int productId, int sizeId, int colorId, int quantity)
		{
			var inventoryItem = db.Inventories.FirstOrDefault(i => i.ProductId == productId && i.SizeId == sizeId && i.ColorId == colorId);
			if (inventoryItem != null)
			{
				inventoryItem.Quantity = quantity;
				db.SaveChanges();
			}
		}
		public async Task<IActionResult> OnPostAsync(string paymentMethod, string deliverylocatiton, double TotalPrice)
		{
			var cartItems = await db.Carts.Include(p=>p.Product)
				.Where(c => c.UserId == 1)
				.ToListAsync();

            CartCount = await db.Carts
                .Where(c => c.UserId == 1)
                .Select(c => c.Id)
                .Distinct()
                .CountAsync();

            var order = new Order
			{
				UserId = 1,
				PaymentMethod = paymentMethod,
				DeliveryLocation = deliverylocatiton,
				TotalPrice = (decimal)TotalPrice,
				Quantity = CartCount,
				DateOrdered = DateTime.UtcNow,
			};

			// add order to database
			db.Orders.Add(order);
			await db.SaveChangesAsync();

			
			int orderId = order.Id;

			
			foreach (var cartItem in cartItems)
			{
				if (cartItem.Product != null)
				{
					var orderDetail = new OrderDetail
					{
						ProductId = cartItem.ProductId,
						SizeId = cartItem.SizeId,
						ColorId = cartItem.ColorId,
						Quantity = cartItem.Quantity,
						Price = cartItem.Product.Price,
						OrderId = orderId
					};

					db.OrderDetails.Add(orderDetail);

					// update inventory
					var inventoryQuantity = db.Inventories.FirstOrDefault(i => i.ProductId == cartItem.ProductId && i.SizeId == cartItem.SizeId && i.ColorId == cartItem.ColorId)?.Quantity ?? 0;
					UpdateInventory(cartItem.ProductId, cartItem.SizeId, cartItem.ColorId, inventoryQuantity - cartItem.Quantity);
					db.Carts.Remove(cartItem);
				}
			}

			// save changes to the database
			await db.SaveChangesAsync();

			// redirect to order confirmation page
			return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
		}


		public async Task<IActionResult> OrderConfirmation(int orderId)
		{ 		

			//// retrieve order details using orderId
			//var orderDetails = db.OrderDetails.Include(od => od.Product).Where(od => od.OrderId == orderId).ToList();

			// display order details to the user
			return RedirectToPage("/Card/List");
		}
	}
}
