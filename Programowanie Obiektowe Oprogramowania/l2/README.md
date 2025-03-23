## Zad 1
Diagram klas na podstawie kodu
```csharp
public interface ICommand
{
    void Execute( string CommandName );
}
public abstract class AbstractCommand : ICommand
{
    private int commandCount;
    protected string commandState;
    public string commandName;
    private void commandBuilder() { }
    public abstract void Execute( string CommandName );
}
public class ConcreteCommand : AbstractCommand
{
    protected CommandStepBuilder commandBuilder;
    public override void Execute( string CommandName ) { }
}
public class CommandStepBuilder
{
    public const int MAXSTEPS = 10;
    public static int StepCount;
}
```

```mermaid
classDiagram
    class ICommand {
        <<interface>>
        +Execute(string CommandName)
    }

    class AbstractCommand {
        <<abstract>>
        -int commandCount
        #string commandState
        +string commandName
        -commandBuilder()
        +Execute(string CommandName)
    }

    class ConcreteCommand {
        #CommandStepBuilder commandBuilder
        +Execute(string CommandName)
    }

    class CommandStepBuilder {
        +const int MAXSTEPS = 10
        +static int StepCount
    }

    ICommand <|.. AbstractCommand
    AbstractCommand <|-- ConcreteCommand
    ConcreteCommand --> CommandStepBuilder : uses

```

## Zad 2
Diagram obiektów
```mermaid
classDiagram
    class `ConcreteCommand : cmd1` {
        commandName: "StartProcess"
        commandState: "Initialized"
    }

    class `ConcreteCommand : cmd2` {
        commandName: "StopProcess"
        commandState: "Executed"
    }

    class `CommandStepBuilder : stepBuilder` {
        MAXSTEPS: 10
        StepCount: 3
    }

    `ConcreteCommand : cmd1` --> `CommandStepBuilder : stepBuilder` : uses
    `ConcreteCommand : cmd2` --> `CommandStepBuilder : stepBuilder` : uses
```
## Zad3
Diagram stanów
```mermaid
stateDiagram
    [*] --> SelectingDrink : Oczekiwanie
    SelectingDrink --> Payment : Wybór potwierdzony
    Payment --> Brewing : Płatność zaakceptowana
    Payment --> PaymentFailed : Płatność odrzucona
    PaymentFailed --> SelectingDrink : Anulowanie lub ponowna próba
    Brewing --> Dispensing 
    Brewing --> OutofService : Zwrot Środków 
    OutofService --> SelectingDrink
    Dispensing --> ReceivingADrink
    ReceivingADrink --> SelectingDrink 
```

## Zad4
Diagram czynności (wersja bez partycji)
```mermaid
graph TD
    Start(["Start"]) --> SelectDrink["Wybór napoju"]
    SelectDrink --> InsertMoney["Wpłata środków"]
    
    InsertMoney -- Brak środków --> Cancel["Anulowanie"]
    InsertMoney -- Błędna kwota --> Retry["Ponowna próba"]
    Retry --> InsertMoney
    InsertMoney -- Prawidłowa płatność --> PaymentSuccess["Płatność zaakceptowana"]
    
    PaymentSuccess --> Brewing["Przygotowanie napoju"]
    Brewing --> Dispensing["Wydawanie napoju"]
    Dispensing --> TakeDrink["Odbiór napoju"]
    TakeDrink --> End(["Koniec"])

    Cancel --> End

```

Diagram czynności (wersja z partycjami)
```mermaid
graph TD
    subgraph Użytkownik
        StartU(["Start"]) --> SelectDrink["Wybór napoju"]
        SelectDrink --> Pay["Wpłata środków"]
        Pay -- Brak środków --> CancelU["Anulowanie"]
        Pay -- Błędna kwota --> RetryU["Ponowna próba"]
        RetryU --> Pay
        TakeDrink["Odbiór napoju"] --> EndU(["Koniec"])
    end

    subgraph Automat do kawy
        PaymentProcessing["Przetwarzanie płatności"] -- Prawidłowa kwota --> PaymentSuccess["Płatność zaakceptowana"]
        PaymentSuccess --> Brewing["Przygotowanie napoju"]
        Brewing --> Dispensing["Wydawanie napoju"]
        Dispensing --> ReadyToTake["Napój gotowy"]
        ReadyToTake --> TakeDrink
        PaymentProcessing -- Błąd płatności --> CancelA["Zwrot środków"]
    end

    Pay --> PaymentProcessing
    CancelU --> EndU
    CancelA --> CancelU
    ReadyToTake --> TakeDrink

```

## Zad5
Diagram sekwencji
```mermaid
sequenceDiagram
    participant Użytkownik
    participant UI as Interfejs Użytkownika
    participant DB as Repozytorium Danych

    Użytkownik ->> UI: Wprowadza dane rejestracyjne
    UI ->> UI: Walidacja danych
    alt Błędne dane
        UI -->> Użytkownik: Komunikat o błędzie
    else Poprawne dane
        UI ->> DB: Sprawdzenie unikalności e-maila
        alt E-mail już istnieje
            DB -->> UI: Błąd - e-mail zajęty
            UI -->> Użytkownik: Komunikat o błędzie
        else E-mail unikalny
            UI ->> DB: Zapis nowego konta
            DB -->> UI: Potwierdzenie zapisu
            UI -->> Użytkownik: Konto utworzone, potwierdź e-mail
        end
    end
```

## Zad6
Diagram sekwencji na podstawie kodu
```csharp
public class Zadanie1 {
    A a;
    public void Diagram( int v ) {
        a.Wykonaj( v );
    } 
}
public class A {
    B b; C c;
    public void Wykonaj( int x ) {
        if ( x < 10 ) {
            b.Oblicz( x );
        else
            c.Oblicz( x );
        }
    } 
}
public class B {
    public void Oblicz( int n ) { }
}
public class C {
    public void Oblicz( int m ) { }
}
```

```mermaid
sequenceDiagram
    participant Z as Zadanie1
    participant A as Klasa A
    participant B as Klasa B
    participant C as Klasa C

    Z ->> A: Diagram(v)
    A ->> A: Sprawdzenie warunku (v < 10)

    alt v < 10
        A ->> B: Oblicz(v)
    else v >= 10
        A ->> C: Oblicz(v)
    end
```
Na podstawie kodu, mozna narysowac precyzyjny diagram. Wynika to z tego, ze mamy os czasu i kazda klase. Rysujemy taka sciezke, ktora wedruje w zaleznosci od czasu i parametrow.

## Zad7

Kod na podstawie diagramu sekwencji
![Moje zdjęcie](./zad7.png)
