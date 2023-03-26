using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shoes_Shop.Models;

namespace Shoes_Shop.Pages.Admin.ManageProduct
{
    public class EditModel : PageModel
    {
        private readonly ShoesShopContext _context;

        public EditModel(ShoesShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

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
           ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile image, string category, string name, string price, string description, int idEdit)
        {
            Product p = await _context.Products.FirstOrDefaultAsync(m => m.Id == idEdit);
            try
            {
                string fileName = Path.GetFileName(image.FileName);
                string typeFile = fileName.Split(".")[1];
                string nameOfImg = $"Product{idEdit}.{typeFile}";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "ImageProduct", nameOfImg);

                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                p.Name = name;
                p.Price = Convert.ToDecimal(price);
                p.Description = description;
                p.CategoryId = Convert.ToInt32(category);
                p.Image = nameOfImg;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
