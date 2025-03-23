// liskov substitution principle - Musi istnieć możliwość zastępowania typów bazowych ich podtypami (również w kontekście
// semantycznym: poprawności działania programu, a nie tylko syntaktycznym: program się
// skompilował)

// ochrona aprzed naruszeniem lsp - wolno osłabić warunek wejścia
// (precondition) lub wzmocnić warunek wyjścia (postcondition) w przeciążanych metodach.

// problemy : ustawienie wymiarow prostokata, obliczanie pola prostokata

public interface IShape
{
    int CalculateArea();
}

public class Rectangle : IShape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Rectangle(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public int CalculateArea()
    {
        return Width * Height;
    }
}

public class Square : IShape
{
    public int Side { get; set; }

    public Square(int side)
    {
        Side = side;
    }

    public int CalculateArea()
    {
        return Side * Side;
    }
}

// Klasa obliczająca pole
public class AreaCalculator
{
    public int CalculateArea(IShape shape)
    {
        return shape.CalculateArea();
    }
}

public class Program
{
    public static void Main()
    {
        int w = 4, h = 5;
        
        IShape rect = new Rectangle(w, h);
        AreaCalculator calc = new AreaCalculator();
        Console.WriteLine("Prostokąt o wymiarach {0} na {1} ma pole {2}", w, h, calc.CalculateArea(rect));

        int side = 5;
        IShape square = new Square(side);
        Console.WriteLine("Kwadrat o boku {0} ma pole {1}", side, calc.CalculateArea(square));
    }
}

