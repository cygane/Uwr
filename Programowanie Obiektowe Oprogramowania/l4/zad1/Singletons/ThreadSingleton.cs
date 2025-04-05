using System;
using System.Threading;

namespace zad1.Singletons
{
    public class ThreadSingleton
    {
        private static readonly AsyncLocal<ThreadSingleton> _instance = new();

        public static ThreadSingleton Instance
        {
            get
            {
                if (_instance.Value == null)
                {
                    _instance.Value = new ThreadSingleton();
                }
                return _instance.Value;
            }
        }

        public Guid Id { get; } = Guid.NewGuid();

        private ThreadSingleton() { }
    }
}
