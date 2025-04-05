using System;

// jedna instancja dla całego procesu
// Typically, this is accomplished by:
// - Declaring all constructors of the class to be private, which prevents it from being instantiated by other objects
// - Providing a static method that returns a reference to the instance

//jaka jest roznica miedzy instance z getem w srodku a getinstance?

namespace zad1.Singletons
{
    public class ProcessSingleton
    {
        private static readonly Lazy<ProcessSingleton> _instance = 
            new(() => new ProcessSingleton());
        
        public static ProcessSingleton Instance => _instance.Value;

        public Guid Id { get; } = Guid.NewGuid();

        private ProcessSingleton() { }
    }
}
