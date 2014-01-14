using System;

namespace Harmony.Sdk.Reactive
{
    public class AbstractObservable<TType> : IObservable<TType>
    {
        Func<IObserver<TType>, IDisposable> _subscriber;

        public AbstractObservable(Func<IObserver<TType>, IDisposable> subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException("subscriber");

            _subscriber = subscriber;
        }

        #region IObservable implementation

        public IDisposable Subscribe(IObserver<TType> observer)
        {
            return _subscriber(observer);
        }

        #endregion
    }
}

