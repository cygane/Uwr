# Single Responsibility Principle
Klasa ma tylko jedną odpowiedzialność

```csharp
public class ReportPrinter
{
    public string GetData();
    public void FormatDocument();
    public void PrintReport();
}
```

test SRP:
- drukarka raportów pozyskuje dane sama !
- drukarka raportów formatuje dokumenty sama !
- drukarka raportów drukuje raporty sama

```csharp
public class DataFetcher
{
    public string GetData(){}
}

public class DocumentFormatter
{
    public string FormatDocument(string data){}
}

public class ReportPrinter
{
    public void PrintReport(string formattedDocument){} 
}

```