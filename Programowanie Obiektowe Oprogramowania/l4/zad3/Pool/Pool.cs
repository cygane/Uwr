
namespace Pool{

    public class Reusable
    {
        public void DoWork(){}
    }
    
    public class BetterReusable
    {
        private Reusable _reusable;
        private bool _isReleased = false;

        // Konstruktor próbuje pozyskać zasób z puli
        public BetterReusable()
        {
            _reusable = ObjectPool.Instance.AcquireReusable();
        }

        // Metoda uwalniająca zasób
        public void Release()
        {
            if (_isReleased)
            {
                throw new InvalidOperationException("The resource has already been released.");
            }
            
            ObjectPool.Instance.ReleaseReusable(_reusable);
            _isReleased = true;
        }

        // Wszystkie operacje są delegowane do obiektu Reusable
        public void DoWork()
        {
            if (_isReleased)
            {
                throw new InvalidOperationException("Cannot perform work on a released resource.");
            }

            _reusable.DoWork();
        }
    }

    public class ObjectPool
    {
        int _poolSize;
        List<Reusable> _pool = new List<Reusable>();
        List<Reusable> _acquired = new List<Reusable>();
        public static ObjectPool Instance { get; } = new ObjectPool(5);

        public ObjectPool( int PoolSize )
        {
            if ( PoolSize <= 0 )
            {
                throw new ArgumentException("Value must be greater than zero.", nameof(PoolSize));
            }
            this._poolSize = PoolSize;
        }
        public Reusable AcquireReusable()
        {
            if ( _acquired.Count() == this._poolSize )
            {
                throw new ArgumentException("Pool is already full.");
            }
            // tworzenie elementu jeśli pool pusty
            if ( _pool.Count() == 0 )
            {
                var reusable = new Reusable();
                _pool.Add( reusable );
            }

            var element = _pool[0];
            _pool.Remove( element );
            _acquired.Add( element );
            return element;
        }
        public void ReleaseReusable( Reusable reusable )
        {
            if ( !_acquired.Contains( reusable ) )
            {
                throw new ArgumentException("The resource was not acquired from the pool.");
            }

            _acquired.Remove( reusable );
            _pool.Add( reusable );
        }
    }
}
