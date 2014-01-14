using System;

namespace Harmony.Sdk.Reactive
{
    public class AbstractObservable<TType, TResult> : IObservable<TType, TResult>
    {
        Func<IObserver<TType, TResult>, IDisposable> _subscriber;
        
        public AbstractObservable(Func<IObserver<TType, TResult>, IDisposable> subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException("subscriber");
            
            _subscriber = subscriber;
        }
        
        #region IObservable implementation
        
        public IDisposable Subscribe(IObserver<TType, TResult> observer)
        {
            return _subscriber(observer);
        }
        
        #endregion
    }
}