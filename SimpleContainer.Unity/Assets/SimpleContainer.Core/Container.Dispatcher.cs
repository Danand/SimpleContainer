using System;
using System.Linq;
using System.Reflection;

using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    public sealed partial class Container
    {
        [Obsolete("Don't use a signal bus!")]
        public void RegisterEvent<TEventArgs>()
            where TEventArgs : IEventArgs
        {
            Action<IEventHandler<TEventArgs>, TEventArgs> action = (handler, args) =>
            {
                handler.OnEvent(args);
            };

            dispatcher.RegisterEvent(this, action);
        }

        [Obsolete("Don't use a signal bus!")]
        public void RegisterEvent<TEventHandler, TEventArgs>()
        {
            Action<TEventHandler, TEventArgs> action = (handler, args) =>
            {
                var foundMethod = typeof(TEventHandler).GetMethods().FirstOrDefault(method => CheckParameters(method, typeof(TEventArgs)));

                if (foundMethod == null)
                    throw new MissingMethodException();

                foundMethod.Invoke(handler, new object[] { args });
            };

            dispatcher.RegisterEvent(this, action);
        }

        [Obsolete("Don't use a signal bus!")]
        public void RegisterEvent<TEventHandler, TEventArgs>(Action<TEventHandler, TEventArgs> action)
        {
            dispatcher.RegisterEvent(this, action);
        }

        [Obsolete("Don't use a signal bus!")]
        public void SendEvent<TEventArgs>(TEventArgs eventArgs)
        {
            dispatcher.Send(eventArgs);
        }

        private bool CheckParameters(MethodInfo method, Type parameterType)
        {
            var parameters = method.GetParameters();

            if (parameters.Length != 1)
                return false;

            return parameters[0].ParameterType == parameterType;
        }
    }
}