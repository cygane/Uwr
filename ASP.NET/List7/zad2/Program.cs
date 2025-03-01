using zad2.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja bazy danych SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfiguracja autentykacji na bazie ciastek
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();

// Odpowiedz na pytania:
// (a) dlaczego po stronie serwera hasła użytkownika nie można przechować w postaci jawnej?
// (b) dlaczego niektóre funkcje skrótu (które?) są niewskazane w praktyce?
// (c) do czego potrzebna jest dodatkowa wartość (salt) przy wyliczaniu skrótu?
// (d) dlaczego hasła przechowuje się w osobnej tabeli i nie wystarczy do tego kolumna (kolumny) w tej samej tabeli w której przechowuje się listę użytkowników?
// (e) jakie mechanizmy ochrony przed atakami typu brute-force można zastosować w ty- powej aplikacji internetowej?
// (f) jak obsłużyć scenariusz w którym użytkownik zapomniał hasła i chce je w jakiś sposób odzyskać?

// (a) administartor ma dostep do poufnych informacji, przy wycieku danych uzytkownicy sa narazeni na natychmiastowe przejecie kont
// (b) MD5 i SHA-1, poniewaz zostały zaprojektowane do szybkiego przetwarzania, co umożliwia ich wykorzystanie w atakach brute-force 
//     i przy generowaniu tęczowych tablic (rainbow tables), brak odpornosci na kolizje
// (c) atakujacy musi zlamac kazdy skrot osobno, uzytkownicy z tym samym haslem maja inny skrot
// (d) minimalizuje ryzyko w przypadku sql injection
// (e) wymuszanie silnych haseł, captcha, blokowanie konta po kilku nieudanych probach logowania, uzycie bcrypt
// (f) uzytkownik wprowadza mail, system generuje unikalny,jednorazowy i z ograniczona waznoscia token i wysyla maila, 
//     jesli token jest poprawny system pozwala zresetowac haslo

