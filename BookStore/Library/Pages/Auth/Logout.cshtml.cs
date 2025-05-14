using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.SignOutAsync("MyCookieAuthenticationScheme");
            HttpContext.Session.Clear();
            return RedirectToPage("/Auth/Login");
        }
    }
}
