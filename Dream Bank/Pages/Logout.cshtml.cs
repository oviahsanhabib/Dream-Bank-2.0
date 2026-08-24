using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Dream_Bank.Pages
{
    public class LogoutModel : PageModel
    {
        public bool IsLoggedIn { get; set; }
        public void OnGet()
        {
            IsLoggedIn = User.Identity?.IsAuthenticated ?? false;
        }
        public async Task<IActionResult> OnPostLogout()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            }

            
            return RedirectToPage("/Login");
        }

        public IActionResult OnPostDontLogout()
        {
           
            return RedirectToPage("/Home");
        }
    }
}
