using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using zad1.Singletons;
using Xunit;

namespace zad1Tests.Tests
{
    public class zad1Tests
    {
        [Fact]
        public void ProcessSingleton_ShouldReturnSameInstance()
        {
            var instance1 = ProcessSingleton.Instance;
            var instance2 = ProcessSingleton.Instance;

            Assert.Equal(instance1.Id, instance2.Id);
        }

        // [Fact]
        // public void ThreadSingleton_ShouldReturnDifferentInstancesForDifferentThreads()
        // {
        //     ConcurrentBag<Guid> threadIds = new();
            
        //     Parallel.For(0, 10, _ =>
        //     {
        //         threadIds.Add(ThreadSingleton.Instance.Id);
        //     });

        //     Assert.Equal(10, threadIds.Distinct().Count());
        // }
        [Fact]
        public void ThreadSingleton_ShouldReturnDifferentInstancesForDifferentThreads()
        {
            ConcurrentBag<Guid> threadIds = new();
            var tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    threadIds.Add(ThreadSingleton.Instance.Id);
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Assert.Equal(10, threadIds.Distinct().Count());
        }



        [Fact]
        public void TimedSingleton_ShouldReturnSameInstanceWithin5Seconds()
        {
            var instance1 = TimedSingleton.Instance;
            Thread.Sleep(3000);
            var instance2 = TimedSingleton.Instance;

            Assert.Equal(instance1.Id, instance2.Id);
        }

        [Fact]
        public void TimedSingleton_ShouldCreateNewInstanceAfter5Seconds()
        {
            var instance1 = TimedSingleton.Instance;
            Thread.Sleep(6000);
            var instance2 = TimedSingleton.Instance;

            Assert.NotEqual(instance1.Id, instance2.Id);
        }
    }
}
