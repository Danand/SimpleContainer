using System;
using System.Collections.Generic;
using System.Linq;

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

        internal void RegisterEvent<TEventHandler, TEventArgs>(Container container, Action<TEventHandler, TEventArgs> action)
        {
            if (!container.CheckRegistered<Dispatcher>())
                container.Register(Scope.Singleton, this);

            var eventArgsType = typeof(TEventArgs);

            if (!events.ContainsKey(eventArgsType))
                events.Add(eventArgsType, new List<Action<object>>());

            events[eventArgsType].Add(args => Invoke(container, action, args));
        }

        private static void Invoke<TEventHandler, TEventArgs>(
            Container                           container,
            Action<TEventHandler, TEventArgs>   action,
            object                              args)
        {
            var eventHandlerType = typeof(TEventHandler);
            var allCachedInstances = container.GetAllCached();
            var eventHandlers = allCachedInstances.Where(instance => eventHandlerType.IsInstanceOfType(instance)).ToArray();

            foreach (var eventHandler in eventHandlers)
                action.Invoke((TEventHandler)eventHandler, (TEventArgs)args);
        }
    }
}