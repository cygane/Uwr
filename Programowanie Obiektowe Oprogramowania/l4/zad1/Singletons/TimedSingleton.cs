using System;

namespace zad1.Singletons
{
    public class TimedSingleton
    {
        private static TimedSingleton _instance;
        private static readonly object _lock = new();
        private static DateTime _lastCreationTime;

        public static TimedSingleton Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null || (DateTime.UtcNow - _lastCreationTime).TotalSeconds > 5)
                    {
                        _instance = new TimedSingleton();
                        _lastCreationTime = DateTime.UtcNow;
                    }
                    return _instance;
                }
            }
        }

        public Guid Id { get; } = Guid.NewGuid();

        private TimedSingleton() { }
    }
}
