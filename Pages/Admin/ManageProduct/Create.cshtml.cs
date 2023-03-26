using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_Shop.Models;

namespace Shoes_Shop.Pages.Admin.ManageProduct
{
    public class CreateModel : PageModel
    {
        private readonly ShoesShopContext _context;

        public CreateModel(ShoesShopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public void OnPostCreateProduct(IFormFile image, string category, string name, string price, string description)
        {
            try
            {
                int id = _context.Products.OrderByDescending(x => x.Id).FirstOrDefault().Id;

                string fileName = Path.GetFileName(image.FileName);
                string typeFile = fileName.Split(".")[1];
                string nameOfImg = $"Product{id + 1}.{typeFile}";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "ImageProduct", nameOfImg);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                Product product = new Product()
                {
                    Name = name,
                    Price = Convert.ToDecimal(price),
                    Description = description,
                    CategoryId = Convert.ToInt32(category),
                    Image = nameOfImg
                };
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
