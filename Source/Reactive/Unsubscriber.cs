using System;
using System.Collections.Generic;

namespace Hive.SeedWorks.Reactive
{
    public class Unsubscriber<T> : IDisposable
    {
        private readonly List<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        public Unsubscriber(
            List<IObserver<T>> observers,
            IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose() => _observers?.Remove(_observer);

        public static IDisposable Subscribe<T>(List<IObserver<T>> observers, IObserver<T> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }

            return new Unsubscriber<T>(observers, observer);
        }
    }
}