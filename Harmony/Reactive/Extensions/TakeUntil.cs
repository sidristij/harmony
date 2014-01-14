using System;

namespace Harmony.Sdk.Reactive.Extensions
{
    public static partial class Extensions
    {
        internal class TakeUntilObservableFilter<TType> : AbstractObservableFilter<TType>
        {
            IDisposable _targetToken, _untilToken;
            IObservable<TType> _target, _until;

            public TakeUntilObservableFilter(IObservable<TType> target, IObservable<TType> until) : base()
            {
                _target = target;
                _until = until;
                StartProcessing();
            }

            protected virtual void StartProcessing()
            {
                _targetToken = _target.Subscribe(this);
                _untilToken = _until.Subscribe(Observer.Create(
                delegate {
                    Unsubscribe();
                }));
            }

            protected virtual void Unsubscribe()
            {
                if (_targetToken != null)                
                    _targetToken.Dispose();
                if (_untilToken != null)
                    _untilToken.Dispose();
            }

            public override void Reset()
            {
                Unsubscribe();
                CheckForFilterAndReset(_target);
                CheckForFilterAndReset(_until);
                StartProcessing();
            }
        }

        public static AbstractObservableFilter<TType> TakeUntil<TType>(this IObservable<TType> target, IObservable<TType> until)
        {
            return new TakeUntilObservableFilter<TType>(target, until);
        }
    }
}

