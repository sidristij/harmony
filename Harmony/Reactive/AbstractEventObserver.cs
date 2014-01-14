using System;
using System.Collections.Generic;
using System.Threading;

namespace Harmony.Sdk.Reactive
{
    public class ObserverEventArgs<TEventArgsType>
    {
        internal ObserverEventArgs(object sender, TEventArgsType eventArgs)
        {
            Sender = sender;
            EventArgs = eventArgs;
        }

        public TEventArgsType EventArgs { get; private set; }

        public object Sender { get; private set; }
    }

    public class AbstractEventObservable<TEventArgsType> : AbstractObservable<ObserverEventArgs<TEventArgsType>>
    {
        object _target;
        string _eventName;
        List<EventSubscriberToken<TEventArgsType>> _tokensList;

        ReaderWriterLockSlim _rwlock;

        public AbstractEventObservable(object target, string eventName) : base(SubscriberFunction)
        {
            _rwlock = new ReaderWriterLockSlim();
            _tokensList = new List<EventSubscriberToken<TEventArgsType>>();
            var targetType = target.GetType();
            var eventInfo = targetType.GetEvent(eventName);
            var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, "OnEventIncoming");
            eventInfo.AddEventHandler(target, handler);
        }

        void OnEventIncoming(object sender, TEventArgsType eventArgs)
        {
            _rwlock.EnterReadLock();
            try
            {
                foreach (var token in _tokensList)
                {
                    token.Observer.OnNext(eventArgs);
                }
            }
            finally
            {
                _rwlock.ExitReadLock();
            }
        }

        IDisposable SubscriberFunction(IObserver<ObserverEventArgs<TEventArgsType>> observer)
        {
            _rwlock.EnterWriteLock();
            try
            {
                var token = new EventSubscriberToken<ObserverEventArgs<TEventArgsType>>(this, observer);
                _tokensList.Add(token);
                return token;
            }
            finally
            {
                _rwlock.ExitWriteLock();
            }
        }

        void Unsubscribe(EventSubscriberToken<TEventArgsType> token)
        {
            _rwlock.EnterWriteLock();
            try
            {
                _tokensList.Remove(token);
            }
            finally
            {
                _rwlock.ExitWriteLock();
            }
        }

        class EventSubscriberToken<TEventArgsType> : IDisposable
        {
            AbstractEventObservable<TEventArgsType> _parent;

            public EventSubscriberToken(AbstractEventObservable<TEventArgsType> parent, IObserver<ObserverEventArgs<TEventArgsType>> observer)
            {
                _parent = parent;
                Observer = observer;
            }

            public IObserver<ObserverEventArgs<TEventArgsType>> Observer { get; private set; }

            void Dispose()
            {
                _parent.Unsubscribe(this);
                Observer = _parent = null;
                GC.SuppressFinalize(this);
            }
        }
    }
}

