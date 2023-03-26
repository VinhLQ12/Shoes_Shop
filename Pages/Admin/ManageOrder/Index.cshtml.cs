using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shoes_Shop.Models;

namespace Shoes_Shop.Pages.Admin.ManageOrder
{
    [Authorize(AuthenticationSchemes = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ShoesShopContext _context;

        public IndexModel(ShoesShopContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; }

        public async Task OnGetAsync()
        {
            Order = await _context.Orders
                .Include(o => o.User).ToListAsync();
        }
    }
}
