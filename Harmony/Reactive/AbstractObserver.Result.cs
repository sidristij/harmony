using System;

namespace Harmony.Sdk.Reactive
{
    public class AbstractObserver<TType, TResult> : IObserver<TType, TResult>
    {
        Func<TResult>            _completed;
        Func<Exception, TResult> _error;
        Func<TType, TResult>     _next;
        
        public AbstractObserver(Func<TType,TResult> next, Func<TResult> completed, Func<Exception, TResult> error)
        {
            _next = next;
            _completed = completed;
            _error = error;
        }
        
        public AbstractObserver(Func<TType, TResult> next)
        {
            _next = next;
            _completed = OnCompletedStub;
            _error = OnErrorStub;
        }
        
        #region IObserver implementation
        
        public virtual TResult OnCompleted()
        {
            return _completed();
        }
        
        public virtual TResult OnError(Exception error)
        {
            return _error(error);
        }
        
        public virtual TResult OnNext(TType value)
        {
            return _next(value);
        }
        
        #endregion
        
        public static TResult OnCompletedStub()
        {
            return default(TResult);
        }
        
        public static TResult OnNextStub(TType value)
        {
            return default(TResult);
        }
        
        public static TResult OnErrorStub(Exception error)
        {
            return default(TResult);
        }
    }
}

