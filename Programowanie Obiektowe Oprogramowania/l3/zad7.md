# Dependency Inversion Principle
Moduły wysokiego poziomu nie powinny zależeć od modułów niskiego poziomu tylko od
abstrakcji.
Abstrakcje nie powinny zależeć od szczegółowych rozwiązań.

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

po wprowadzeniu klasey ReportComposer która obsługuje wstrzykiwanie zależności do obiektów usługowych

```csharp 
public interface IDataFetcher
{
    string GetData();
}

public interface IDocumentFormatter
{
    string FormatDocument(string data);
}

public interface IReportPrinter
{
    void PrintReport(string formattedDocument);
}

public class DataFetcher : IDataFetcher
{
    public string GetData(){}
}

public class DocumentFormatter : IDocumentFormatter
{
    public string FormatDocument(string data){}
}

public class ReportPrinter : IReportPrinter
{
    public void PrintReport(string formattedDocument){}
}

// odpowiada za połączenie różnych usług (pobierania danych, formatowania i drukowania)
public class ReportComposer
{
    private readonly IDataFetcher _dataFetcher;
    private readonly IDocumentFormatter _documentFormatter;
    private readonly IReportPrinter _reportPrinter;

    public ReportComposer(IDataFetcher dataFetcher, IDocumentFormatter documentFormatter, IReportPrinter reportPrinter)
    {
        _dataFetcher = dataFetcher;
        _documentFormatter = documentFormatter;
        _reportPrinter = reportPrinter;
    }

    public void ComposeAndPrintReport()
    {
        string data = _dataFetcher.GetData();
        string formattedDocument = _documentFormatter.FormatDocument(data);
        _reportPrinter.PrintReport(formattedDocument);
    }
}
```