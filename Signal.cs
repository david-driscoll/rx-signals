using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace rx_signals
{
    public class Signal<T> : IDisposable, IObservable<T>
    {
        private BehaviorSubject<T> _context;
        private IDisposable _subscription;

        internal Signal(IObservable<T> source) : this(source, default)
        {
        }

        internal Signal(IObservable<T> source, T value)
        {
            _context = new BehaviorSubject<T>(value);
            _subscription = source
                .ObserveOn(Scheduler.Immediate)
                // do not want to complete if the source completes
                .Subscribe(_context.OnNext, _context.OnError);
        }

        public T Value
        {
            get => Volatile.Read(ref _context).Value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_context.Value, value)) return;
                _context.OnNext(value);
            }
        }

        // public ISignalSnapshot<T> GetSnapshot() => Volatile.Read(ref _snapshot);

        public void Dispose()
        {
            _subscription.Dispose();
            _context.Dispose();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _context.Subscribe(observer);
        }
    }

    public static class SignalExtensions
    {
        public static Signal<T> ToSignal<T>(this IObservable<T> source) => new Signal<T>(source);
    }
}
