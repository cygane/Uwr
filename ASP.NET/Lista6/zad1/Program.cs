using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja usług
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<CMSCustomRouteTransformer>();

var app = builder.Build();

// Konfiguracja middleware
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // Rejestracja dynamicznego routingu dla CMS
    endpoints.MapDynamicControllerRoute<CMSCustomRouteTransformer>("CMS/{**sitepage}");

    // Domyślny routing MVC
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();

