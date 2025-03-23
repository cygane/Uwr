// open-colsed principle - Składniki oprogramowania (klasy, moduły) powinny być otwarte (na rozszerzenia,
// adaptowalne) i zamknięte (na modyfikacje wpływające na klientów)

// public class TaxCalculator {
//     public Decimal CalculateTax(Decimal Price) { 
//         return Price * 0.22; 
//     }
// }

// public class Item {
//     public Decimal Price { get; }
//     public string Name { get; }
// }

// public class CashRegister {
//     public TaxCalculator taxCalc = new TaxCalculator();
    
//     public Decimal CalculatePrice(Item[] Items) {
//         Decimal _price = 0;
//         foreach (Item item in Items) {
//             _price += item.Price + taxCalc.CalculateTax(item.Price);
//         }
//         return _price;
//     }
    
//     public string PrintBill(Item[] Items) {
//         foreach (var item in Items)
//             Console.WriteLine("towar {0} : cena {1} + podatek {2}", item.Name, item.Price, taxCalc.CalculateTax(item.Price));
//     }
// }

// w tej implementacji - brak mozliwosci zmiany podatku, brak mozliwosci zmiany wydruku 

public class Item
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Item(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

public interface ITaxCalculator {
    decimal CalculateTax(decimal Price);
}

public class StandardTaxCalculator : ITaxCalculator {
    public decimal CalculateTax(decimal Price) { 
        return Price * 0.22M; 
    }
}

public class ReducedTaxCalculator : ITaxCalculator {
    public decimal CalculateTax(decimal Price) { 
        return Price * 0.08M; 
    }
}

public interface IBillPrinter {
    void PrintBill(Item[] Items);
}

public class DefaultBillPrinter : IBillPrinter {
    public void PrintBill(Item[] Items) {
        foreach (var item in Items)
            Console.WriteLine("towar {0} : cena {1} + podatek {2}", item.Name, item.Price, item.Price * 0.22M); 
    }
}

public class SortedBillPrinter : IBillPrinter {
    public void PrintBill(Item[] Items) {
        var sortedItems = Items.OrderBy(item => item.Name).ToArray();
        foreach (var item in sortedItems)
            Console.WriteLine("towar {0} : cena {1} + podatek {2}", item.Name, item.Price, item.Price * 0.22M); 
    }
}

public class CashRegister {
    private readonly ITaxCalculator taxCalc;
    private readonly IBillPrinter billPrinter;
    
    public CashRegister(ITaxCalculator taxCalculator, IBillPrinter billPrinter) {
        taxCalc = taxCalculator;
        this.billPrinter = billPrinter;
    }

    public decimal CalculatePrice(Item[] Items) {
        decimal _price = 0;
        foreach (Item item in Items) {
            _price += item.Price + taxCalc.CalculateTax(item.Price);
        }
        return _price;
    }

    public void PrintBill(Item[] Items) {
        billPrinter.PrintBill(Items);
    }
}

public class Program {
    public static void Main() {
        Item[] items = new Item[] {
            new Item("Produkt A", 100M), 
            new Item("Produkt B", 50M)  
        };


        var cashRegister1 = new CashRegister(new StandardTaxCalculator(), new DefaultBillPrinter());
        Console.WriteLine("Cena i podatek:");
        cashRegister1.PrintBill(items);

        var cashRegister2 = new CashRegister(new ReducedTaxCalculator(), new SortedBillPrinter());
        Console.WriteLine("\nCena i podatek (obniżona stawka, posortowane alfabetycznie):");
        cashRegister2.PrintBill(items);
    }
}


