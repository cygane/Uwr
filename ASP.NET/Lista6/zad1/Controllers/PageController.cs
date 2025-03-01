using Microsoft.AspNetCore.Mvc;

public class PageController : Controller
{
    public IActionResult Render()
    {
        var routeData = this.RouteData.Values;
        string site = routeData[CMSCustomRouteTransformer.SITENAME]?.ToString();
        string page = routeData[CMSCustomRouteTransformer.PAGENAME]?.ToString();

        // Odczyt danych z magazynu lub generowanie modelu
        var model = new PageRenderModel
        {
            Site = site,
            Page = page
        };

        return View(model); // Przekazanie modelu do widoku
    }
}

public class PageRenderModel
{
    public string Site { get; set; }
    public string Page { get; set; }
}
