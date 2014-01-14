using System;
using System.Collections.Generic;

namespace Harmony.Sdk.Reactive
{
    public static class Observer
    {
        public static IObservable<TType> Filter<TType>(this IObservable<TType> self, Predicate<TType> match)
        {
            if (match == null) 
            {
                throw new ArgumentNullException("match");
            }

            if (self == null)
            {
                throw new ArgumentNullException("self");
            }

            var observer = new AbstractObservableFilter<TType>(match);
            self.Subscribe(observer);
            return observer;
        }

        public static IObserver<TType> Create<TType>(Action<TType> action)
        {
            return new AbstractObserver<TType>(action);
        }
    }
}

