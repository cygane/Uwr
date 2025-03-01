using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using zad2.Data;

namespace zad2.Controllers
{
    public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register() => View(new User());

    [HttpPost]
    public IActionResult Register(string username, string email, string password)
    {
        // Hashowanie hasła przy użyciu bcrypt
        var hashedPassword = PasswordService.HashPassword(password);

        // Dodanie użytkownika do bazy
        var user = new User { Username = username, Email = email };
        _context.Users.Add(user);
        _context.SaveChanges();

        // Dodanie rekordu hasła do bazy
        var userPassword = new Password
        {
            UserId = user.Id,
            PasswordHash = hashedPassword,
            PasswordSetDate = DateTime.UtcNow
        };

        _context.Passwords.Add(userPassword);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        // Wyszukanie użytkownika po nazwie użytkownika
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        // Pobranie rekordu hasła
        var userPassword = _context.Passwords.FirstOrDefault(p => p.UserId == user.Id);
        if (userPassword == null || !PasswordService.VerifyPassword(password, userPassword.PasswordHash))
        {
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        // Ustawienie ciasteczka uwierzytelniającego
        HttpContext.Response.Cookies.Append("AuthCookie", user.Id.ToString());
        return RedirectToAction("Index", "Home");
    }
}
}




