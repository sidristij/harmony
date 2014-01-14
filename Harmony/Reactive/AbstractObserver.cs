using System;

namespace Harmony.Sdk.Reactive
{
    /// <summary>
    /// Abstract pipe filter. Should be used when target needs to filter incoming push notifications
    /// </summary>

    public class AbstractObserver<TType> : IObserver<TType>
    {
        Action            _completed;
        Action<Exception> _error;
        Action<TType>     _next;
        
        public AbstractObserver(Action<TType> next, Action completed, Action<Exception> error)
        {
            _next = next;
            _completed = completed;
            _error = error;
        }
        
        public AbstractObserver(Action<TType> next)
        {
            _next = next;
            _completed = OnCompletedStub;
            _error = OnErrorStub;
        }

        #region IObserver implementation

        public virtual void OnCompleted()
        {
            _completed();
        }

        public virtual void OnError(Exception error)
        {
            _error(error);
        }

        public virtual void OnNext(TType value)
        {
            _next(value);
        }

        #endregion

        public static void OnCompletedStub()
        {
        }

        public static void OnNextStub(TType value)
        {
        }

        public static void OnErrorStub(Exception error)
        {
        }
    }
}

