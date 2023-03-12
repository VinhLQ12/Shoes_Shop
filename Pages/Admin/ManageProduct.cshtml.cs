using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


using Shoes_Shop.Models;


namespace Shoes_Shop.Pages.Admin {
    public class ManageProductModel : PageModel {

        private readonly ShoesShopContext db;
        public ManageProductModel(ShoesShopContext _db)
        {
            db = _db;
        }

        public string errorMessage = "";
        public List<Category> categories = new List<Category>();
        public List<Product> products = new List<Product>();

        public void OnGet()
        {
            categories = db.Categories.ToList();
            products = db.Products.Include(p => p.Category).ToList();
        }

        public void OnPostCreateProduct(IFormFile image, string category, string name, string price, string description)
        {
            try
            {
                string fileName = Path.GetFileName(image.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "ImageProduct", fileName);
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
                    Image = $"./ImageProduct/{fileName}"
                };
                db.Products.Add(product);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public IActionResult OnPostDeleteProduct(int id)
        {
            Product product = db.Products.FirstOrDefault(x => x.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            else
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return RedirectToPage("./ManageProduct");
        }

        public IActionResult OnPostUpdateProduct(IFormFile image, string category, string name, string price, string description, int id)
        {
            Product product = db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                if(image != null)
                {
                    string fileName = Path.GetFileName(image.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "ImageProduct", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    product.Image = $"./ImageProduct/{fileName}";
                }

                product.Name = name;
                product.Price = Convert.ToDecimal(price);
                product.Description = description;
                product.CategoryId = Convert.ToInt32(category);
                db.SaveChanges();
            }
            return RedirectToPage("./ManageProduct");
        }

    }
}
