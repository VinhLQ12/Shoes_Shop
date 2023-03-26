using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Shoes_Shop.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Shoes_Shop.Pages.Admin
{
    public class LoginModel : PageModel
    {
        ShoesShopContext db = new ShoesShopContext();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string user, string pass, string ReturnUrl)
        {
            User u = db.Users.FirstOrDefault(x => x.Username == user && x.Password == pass && x.IsSeller == 1);
            if(u != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("User", JsonSerializer.Serialize(u)),
                };
                var identity = new ClaimsIdentity(claims, "Admin");

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("Admin", principal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                });
                return ReturnUrl == null ? Redirect("./ManageOrder") : Redirect(ReturnUrl);
            }
            return Page();
        }
    }
}
