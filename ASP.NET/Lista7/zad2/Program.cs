// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.EntityFrameworkCore;
// using zad2.Data;

// var builder = WebApplication.CreateBuilder(args);

// // Dodaj DbContext
// builder.Services.AddDbContext<AuthDbContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// // Dodaj Identity
// builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
// {
//     options.Password.RequireDigit = true;
//     options.Password.RequireLowercase = true;
//     options.Password.RequireUppercase = true;
//     options.Password.RequiredLength = 8;
// }).AddEntityFrameworkStores<AuthDbContext>();

// // Konfiguracja autentykacji
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.LoginPath = "/Account/Login";
//         options.LogoutPath = "/Account/Logout";
//     });

// builder.Services.AddControllersWithViews();

// var app = builder.Build();

// app.UseHttpsRedirection();
// app.UseStaticFiles();
// app.UseRouting();

// app.UseAuthentication();
// app.UseAuthorization();

// app.MapDefaultControllerRoute();

// app.Run();

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using zad2.Data;

var builder = WebApplication.CreateBuilder(args);

// Dodanie usług do kontenera DI
builder.Services.AddControllersWithViews(); // Obsługa MVC
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=AuthApp.db")); // Konfiguracja SQLite

var app = builder.Build();

// Middleware do obsługi błędów
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware obsługujące statyczne pliki (np. CSS, JS)
app.UseStaticFiles();

// Middleware obsługujące routing
app.UseRouting();

// Middleware uwierzytelniania opartego o ciasteczka
app.Use(async (context, next) =>
{
    var authCookie = context.Request.Cookies["AuthCookie"];
    if (authCookie != null)
    {
        var db = context.RequestServices.GetService<ApplicationDbContext>();
        if (db != null)
        {
            if (int.TryParse(authCookie, out var userId))
            {
                var user = await db.Users.FindAsync(userId);
                if (user != null)
                {
                    // Przechowywanie użytkownika w kontekście HTTP
                    context.Items["User"] = user;
                }
            }
        }
    }
    await next();
});

// Definicja endpointów
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Migracja bazy danych przy starcie (opcjonalne)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Uruchomienie aplikacji
app.Run();

