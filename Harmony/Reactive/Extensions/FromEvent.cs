using System;

namespace Harmony.Sdk.Reactive.Extensions
{
    public static class Observable
    {
        public static AbstractObservableFilter<TType> FromEvent<TType>(object target, string eventName)
        {
            return new AbstractEventObservable<TType>(target, eventName);
        }
    }
}

