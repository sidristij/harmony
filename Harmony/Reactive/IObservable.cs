using System;

namespace Harmony.Sdk.Reactive
{
    public interface IObserver<TType> 
    {
        void OnCompleted();
        void OnError(Exception error);
        void OnNext(TType value);
    }

    public interface IObserver<TType, TResult>
    {
        TResult OnCompleted();
        TResult OnError(Exception error);
        TResult OnNext(TType value);
    }

    public interface IObservable<Type>
    {
        IDisposable Subscribe(IObserver<Type> observer);
    }
    
    public interface IObservable<TType, TResult>
    {
        IDisposable Subscribe(IObserver<TType, TResult> observer);
    }
}

