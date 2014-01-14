using System;

namespace Harmony.Sdk.Reactive.Extensions
{
    public static partial class Extensions
    {
        internal class SkipUntilObservableFilter<TType> : AbstractObservableFilter<TType>
        {
            IDisposable _targetToken, _untilToken;
            IObservable<TType> _target, _until;

            public SkipUntilObservableFilter(IObservable<TType> target, IObservable<TType> until) : base()
            {
                StartProcessing();
            }

            protected virtual void StartProcessing()
            {
                _untilToken = _until.Subscribe(Observer.Create(
                    delegate {
                        _target.Subscribe(this);
                        _untilToken.Dispose();
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

        public static AbstractObservableFilter<TType> SkipUntil<TType>(this IObservable<TType> target, IObservable<TType> until)
        {
            return new TakeUntilObservableFilter<TType>(target, until);
        }
    }
}

