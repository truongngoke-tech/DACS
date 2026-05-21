using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DACS.Models;

namespace DACS.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // GET: hiển thị trang xác nhận
        public void OnGet()
        {
        }

        // POST: logout thật sự
        public async Task<IActionResult> OnPost()
        {
            await _signInManager.SignOutAsync();

            // 🔥 Redirect giống login/register
            return RedirectToPage("/Index", new { area = "" });
        }
    }
}