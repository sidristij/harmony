using System;
using System.Collections.Generic;
using System.Threading;

namespace Harmony.Sdk.Reactive
{
    /// <summary>
    /// Abstract pipe filter. Should be used when target needs to filter incoming push notifications
    /// </summary>
    public class AbstractObservableFilter<TType> : IObservable<TType>, IObserver<TType>
    {
        ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        List<IObserver<TType>> observers = new List<IObserver<TType>>();
        Predicate<TType> _match;

        public AbstractObservableFilter(Predicate<TType> match = null)
        {
            _match = match ?? MatchStub;
        }

        public static bool MatchStub(TType arg)
        {
            return true;
        }

        public virtual void Reset()
        {
            ;
        }

        protected void CheckForFilterAndReset(IObservable<TType> observable)
        {
            var targetAsFilter = observable as AbstractObservableFilter<TType>;
            if (targetAsFilter != null)
            {
                targetAsFilter.Reset();
            }
        }

        /// <summary>
        /// Disposable token for unsybscribing from push notifications
        /// </summary>
        private class DisposableToken : IDisposable
        {
            AbstractObservableFilter<TType> _parent;
            
            public DisposableToken(AbstractObservableFilter<TType> parent, IObserver<TType> observer)
            {
                _parent = parent;
                Observer = observer;
            }

            public IObserver<TType> Observer { get; private set; }
            
            public void Dispose()
            {
                _parent.Unsubscribe(this);
                _parent = null;
                GC.SuppressFinalize(this);
            }
        }

        #region IObservable implementation

        public IDisposable Subscribe(IObserver<TType> observer)
        {
            rwlock.EnterWriteLock();
            observers.Add(observer);
            rwlock.ExitWriteLock();
            return new DisposableToken(this, observer);
        }

        #endregion
        
        private void Unsubscribe(DisposableToken token)
        {
            rwlock.EnterWriteLock();
            observers.Remove(token.Observer);
            rwlock.ExitWriteLock();
        }

        #region IObserver implementation

        public void OnCompleted()
        {
            rwlock.EnterUpgradeableReadLock();

            var copy = observers.ToArray();

            rwlock.ExitUpgradeableReadLock();

            foreach(var observer in copy) 
            {
                observer.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            rwlock.EnterUpgradeableReadLock();

            var copy = observers.ToArray();

            rwlock.ExitUpgradeableReadLock();

            foreach(var observer in copy)
            {
                observer.OnError(error);
            }
        }

        public void OnNext(TType value)
        {
            rwlock.ExitUpgradeableReadLock();

            var copy = observers.ToArray();

            rwlock.ExitUpgradeableReadLock();

            foreach(var observer in copy)
            {
                if(_match != null && _match(value)) {
                    observer.OnNext(value);
                }
            }
        }

        #endregion
    }    
}
