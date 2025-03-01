using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class FileDataService
{
    private readonly string _filePath;

    // Konstruktor przyjmujący ścieżkę do pliku
    public FileDataService(IWebHostEnvironment environment)
    {
        _filePath = Path.Combine(environment.WebRootPath, "products.json");
    }

    // Metoda zwracająca listę produktów z pliku JSON
    public async Task<List<Product>> GetProductsAsync()
    {
        if (!File.Exists(_filePath))
            return new List<Product>();

        var jsonData = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Product>>(jsonData);
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}
