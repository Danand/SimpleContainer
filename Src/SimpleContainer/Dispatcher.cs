using System;
using System.Collections.Generic;

namespace SimpleContainer
{
    public class Dispatcher
    {
        private readonly Dictionary<Type, List<Action<object>>> events = new Dictionary<Type, List<Action<object>>>();

        public void Send<TEventArgs>(TEventArgs eventArgs)
        {
            var eventArgsType = typeof(TEventArgs);

            foreach (var callback in events[eventArgsType])
                callback.Invoke(eventArgs);
        }

        internal void RegisterEvent<TEventHandler, TEventArgs>(SimpleContainer container, Action<TEventHandler, TEventArgs> action)
        {
            if (!container.CheckRegistered<Dispatcher>())
                container.Register(Scope.Singleton, this);

            var eventArgsType = typeof(TEventArgs);

            if (!events.ContainsKey(eventArgsType))
                events.Add(eventArgsType, new List<Action<object>>());

            events[eventArgsType].Add(args => Invoke(container, action, args));
        }

        private static void Invoke<TEventHandler, TEventArgs>(
            SimpleContainer                     container,
            Action<TEventHandler, TEventArgs>   action,
            object                              args)
        {
            // TODO: replace `Resolve<>()` with something like `GetCached<>()`.
            var eventHandler = container.Resolve<TEventHandler>();
            action.Invoke(eventHandler, (TEventArgs)args);
        }
    }
}