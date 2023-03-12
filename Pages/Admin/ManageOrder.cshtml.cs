using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Shoes_Shop.Models;

namespace Shoes_Shop.Pages.Admin
{
    public class ManageOrderModel : PageModel
    {
        private readonly ShoesShopContext db;
        public ManageOrderModel(ShoesShopContext _db)
        {
            db = _db;
        }
        public List<Order> Orders { get; set; }
        public void OnGet()
        {
            Orders = db.Orders.Include(x => x.OrderDetails).Include(x => x.User).ToList();
        }
    }
}
