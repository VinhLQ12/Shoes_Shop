using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shoes_Shop.Models;

namespace Shoes_Shop.Pages.Admin.ManageProduct
{
    [Authorize(AuthenticationSchemes = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ShoesShopContext _context;

        public DeleteModel(ShoesShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public string ErrorMessage = string.Empty;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products
                .Include(p => p.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                Product = await _context.Products.FindAsync(id);

                if (Product != null)
                {
                    _context.Products.Remove(Product);
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("./Index");
            }
            catch
            {
                ErrorMessage = "Product is already have order, cant delete!";
                return Page();
            }
        }
    }
}
