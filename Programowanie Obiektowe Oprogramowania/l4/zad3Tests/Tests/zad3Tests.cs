using Pool;
using Xunit;
using System;

namespace zad3Tests
{
    public class zad3Tests
    {
        [Fact]
        public void ShouldWorkWithReusable()
        {
            var betterReusable = new BetterReusable();
            
            // Operacja powinna się udać, bo zasób nie został jeszcze zwrócony
            betterReusable.DoWork();
        }

        [Fact]
        public void ShouldThrowExceptionWhenDoWorkAfterRelease()
        {
            var betterReusable = new BetterReusable();
            
            // Pierwsze wywołanie DoWork powinno się udać
            betterReusable.DoWork();
            
            // Zwracamy zasób
            betterReusable.Release();
            
            // Drugie wywołanie DoWork powinno rzucić wyjątek
            var exception = Assert.Throws<InvalidOperationException>(() => betterReusable.DoWork());
            Assert.Equal("Cannot perform work on a released resource.", exception.Message);
        }

        [Fact]
        public void ShouldThrowExceptionWhenReleaseTwice()
        {
            var betterReusable = new BetterReusable();
            
            // Pierwsze wywołanie Release powinno się udać
            betterReusable.Release();
            
            // Drugie wywołanie Release powinno rzucić wyjątek
            var exception = Assert.Throws<InvalidOperationException>(() => betterReusable.Release());
            Assert.Equal("The resource has already been released.", exception.Message);
        }

        [Fact]
        public void InvalidSize()
        {
            // Testowanie, gdy rozmiar puli jest nieprawidłowy
            var exception = Assert.Throws<ArgumentException>(() => new ObjectPool(0));
            Assert.Equal("Value must be greater than zero. (Parameter 'PoolSize')", exception.Message); // Oczekiwana wiadomość wyjątku, jeśli jest ustawiona w konstruktorze
        }

        [Fact]
        public void ValidSize()
        {
            // Testowanie, gdy rozmiar puli jest prawidłowy
            var pool = new ObjectPool(1);
            var reusable = pool.AcquireReusable();
            Assert.NotNull(reusable);
        }

        [Fact]
        public void CapacityDepleted()
        {
            // Testowanie, gdy pula jest pełna
            var pool = new ObjectPool(1);
            var reusable = pool.AcquireReusable();
            var exception = Assert.Throws<ArgumentException>(() => pool.AcquireReusable());
            Assert.Equal("Pool is already full.", exception.Message); // Oczekiwana wiadomość wyjątku, jeśli jest ustawiona w metodzie AcqureReusable
        }

        [Fact]
        public void ReusedInstance()
        {
            // Testowanie, czy instancja jest zwracana po uwolnieniu
            var pool = new ObjectPool(1);
            var reusable = pool.AcquireReusable();
            pool.ReleaseReusable(reusable);
            var reusable2 = pool.AcquireReusable();
            Assert.Same(reusable, reusable2); // Sprawdzamy, czy obie zmienne wskazują na ten sam obiekt
        }

        [Fact]
        public void ReleaseInvalidInstance()
        {
            // Testowanie, czy próba zwrócenia nieznanej instancji zgłasza wyjątek
            var pool = new ObjectPool(1);
            var reusable = new Reusable(); // Nowa instancja, która nie pochodzi z puli
            var exception = Assert.Throws<ArgumentException>(() => pool.ReleaseReusable(reusable));
            Assert.Equal("The resource was not acquired from the pool.", exception.Message); // Oczekiwana wiadomość wyjątku
        }
    }
}
