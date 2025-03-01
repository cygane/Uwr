using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using zad4.Models;

namespace zad4.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var currentMinute = DateTime.Now.Minute;

        if (currentMinute % 2 == 0)
        {
            ViewData["Layout"] = "_EvenMinuteLayout";
        }
        else
        {
            ViewData["Layout"] = "_OddMinuteLayout";
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
