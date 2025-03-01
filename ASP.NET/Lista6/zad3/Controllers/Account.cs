using Microsoft.AspNetCore.Mvc;
using zad3.Models;

namespace zad3.Controllers
{
	public class Account : Controller
	{
		public IActionResult Login()
		{
			return View(new LoginViewModel());
		}
	}
}