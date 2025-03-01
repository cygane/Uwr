using System.Security.Cryptography;
using System.Text;
using zad2.Data;
using zad2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace zad2.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string name, string email, string password)
        {
            if (_context.Users.Any(u => u.Email == email))
            {
                ModelState.AddModelError("", "Email already in use.");
                return View();
            }

            // Generate salt
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // Generate hash using PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(32);

            // Save user and password
            var user = new User { Name = name, Email = email };
            _context.Users.Add(user);
            _context.SaveChanges();

            var passwordEntry = new Password
            {
                UserId = user.Id,
                Hash = Convert.ToBase64String(hash),
                Salt = Convert.ToBase64String(salt),
                Iterations = 100000
            };
            _context.Passwords.Add(passwordEntry);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View();
            }

            var passwordEntry = _context.Passwords.FirstOrDefault(p => p.UserId == user.Id);
            if (passwordEntry == null)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View();
            }

            var pbkdf2 = new Rfc2898DeriveBytes(password,
                Convert.FromBase64String(passwordEntry.Salt),
                passwordEntry.Iterations);
            var hash = pbkdf2.GetBytes(32);

            if (Convert.ToBase64String(hash) != passwordEntry.Hash)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View();
            }

            // Authentication success
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Name) },
                CookieAuthenticationDefaults.AuthenticationScheme)));

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
