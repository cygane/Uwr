using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class ProductsController : Controller
{
    private readonly FileDataService _fileDataService;

    // Konstruktor z wstrzykiwaniem FileDataService
    public ProductsController(FileDataService fileDataService)
    {
        _fileDataService = fileDataService;
    }

    public async Task<IActionResult> Index()
    {
        // Pobranie produktów z usługi
        var products = await _fileDataService.GetProductsAsync();
        return View(products); // Przekazanie listy produktów do widoku
    }
}
