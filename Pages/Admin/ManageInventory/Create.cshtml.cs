using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_Shop.Models;

namespace Shoes_Shop.Pages.Admin.ManageInventory
{
    public class CreateModel : PageModel
    {
        private readonly Shoes_Shop.Models.ShoesShopContext _context;

        public CreateModel(Shoes_Shop.Models.ShoesShopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorId");
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
        ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeId");
            return Page();
        }

        [BindProperty]
        public Inventory Inventory { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Inventories.Add(Inventory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
