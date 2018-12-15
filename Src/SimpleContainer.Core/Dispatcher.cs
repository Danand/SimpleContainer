using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SimpleContainer.Interfaces;

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

        private void Invoke<TEventHandler, TEventArgs>(
            Container                           container,
            Action<TEventHandler, TEventArgs>   action,
            object                              args)
        {
            var eventHandlerType = typeof(TEventHandler);
            var eventHandlerAnyType = typeof(IEventHandlerAny);
            var anyArgs = new AnyArgs((IEventArgs)args);
            var allCachedInstances = container.GetAllCached();

            var eventHandlersConcrete = allCachedInstances.Where(instance => eventHandlerType.IsInstanceOfType(instance));

            var eventHandlersAny = allCachedInstances.Where(instance => eventHandlerAnyType.IsInstanceOfType(instance) &&
                                                                        !eventHandlersConcrete.Contains(instance));

            foreach (var eventHandler in eventHandlersConcrete)
                action.Invoke((TEventHandler)eventHandler, (TEventArgs)args);

            foreach (var eventHandler in eventHandlersAny)
                ((IEventHandlerAny)eventHandler).OnEvent(anyArgs);
        }

        public IEnumerator CreateYieldInstruction<TEventArgs>()
            where TEventArgs : IEventArgs
        {
            var dynamicEventHandler = new DynamicEventHandler<TEventArgs>();
            var eventArgsType = typeof(TEventArgs);

            if (!events.ContainsKey(eventArgsType))
                events.Add(eventArgsType, new List<Action<object>>());

            events[eventArgsType].Add(args => {});

            while (!dynamicEventHandler.IsCompleted)
                yield return null;
        }

        public IEnumerator CreateYieldInstruction<TEventArgs>(Action<TEventArgs> callback)
            where TEventArgs : IEventArgs
        {
            var dynamicEventHandler = new DynamicEventHandler<TEventArgs>();
            var eventArgsType = typeof(TEventArgs);

            if (!events.ContainsKey(eventArgsType))
                events.Add(eventArgsType, new List<Action<object>>());

            events[eventArgsType].Add(args => callback((TEventArgs)args));

            while (!dynamicEventHandler.IsCompleted)
                yield return null;
        }
    }
}