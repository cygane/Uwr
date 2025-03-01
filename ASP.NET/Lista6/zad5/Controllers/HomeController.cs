using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using zad5.Models;

namespace zad5.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View(new UserModel());
    }

   [HttpPost]
    public IActionResult Index(UserModel model)
    {
        return View(model);
    }
}
