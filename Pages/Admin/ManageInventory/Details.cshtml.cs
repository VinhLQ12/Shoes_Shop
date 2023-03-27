using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shoes_Shop.Models;

namespace Shoes_Shop.Pages.Admin.ManageInventory
{
    public class DetailsModel : PageModel
    {
        private readonly Shoes_Shop.Models.ShoesShopContext _context;

        public DetailsModel(Shoes_Shop.Models.ShoesShopContext context)
        {
            _context = context;
        }

        public Inventory Inventory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Inventory = await _context.Inventories
                .Include(i => i.Color)
                .Include(i => i.Product)
                .Include(i => i.Size).FirstOrDefaultAsync(m => m.Id == id);

            if (Inventory == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
