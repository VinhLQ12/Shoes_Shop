using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Shoes_Shop.Models;

namespace Shoes_Shop.Pages.Admin.ManageOrder {
    [Authorize(AuthenticationSchemes = "Admin")]
    public class DetailsModel : PageModel {
        private readonly ShoesShopContext _context;

        public DetailsModel(ShoesShopContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }
        public List<OrderDetail> OrderDetail = new List<OrderDetail>();


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders
                .Include(o => o.User).FirstOrDefaultAsync(m => m.Id == id);
            OrderDetail = _context.OrderDetails.Include(x => x.Size).Include(x => x.Product).Include(x => x.Color).Where(x => x.OrderId == id).ToList();
            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
