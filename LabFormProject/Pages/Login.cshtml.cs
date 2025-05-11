using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Database.Models;
using System.Text.Json;
//I took lots of the code from Chatgpt
namespace LabFormProject.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string? UserName { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

         public async Task<IActionResult> OnPostAsync()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");

            if (!System.IO.File.Exists(jsonPath))
            {
                ErrorMessage = "User data file not found.";
                return Page();
            }

            var json = await System.IO.File.ReadAllTextAsync(jsonPath);
            var users = JsonSerializer.Deserialize<List<UserData>>(json) ?? new();

            var user = users.FirstOrDefault(u => 
                u.UserName == UserName &&
                u.Password == Password);
            if (user == null)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }
            if (!user.IsActive)
            {
                ErrorMessage = "This user is not active.";
                return Page();
            }
            // Generate simple token
            string token = Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(user.UserName))
            {
                ErrorMessage = "User information is incomplete.";
                return Page();
            }

            // Store in session
            HttpContext.Session.SetString("username", user.UserName);
            HttpContext.Session.SetString("token", token);
            HttpContext.Session.SetString("session_id", HttpContext.Session.Id);

            // Store in cookie
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("username", user.UserName, cookieOptions);
            Response.Cookies.Append("token", token, cookieOptions);
            Response.Cookies.Append("session_id", HttpContext.Session.Id, cookieOptions);

            return Redirect("/Index");
        }

    }
}