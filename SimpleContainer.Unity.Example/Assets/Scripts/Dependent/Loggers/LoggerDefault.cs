using SimpleContainer.Unity.Example.Dependent.Interfaces;

namespace SimpleContainer.Unity.Example.Dependent.Loggers
{
    public sealed class LoggerDefault : ILogger
    {
        private readonly ICultureInfoFormatter cultureInfoFormatter;

        /// <summary>
        /// <see cref="ICultureInfoFormatter"/> is injected here by DI-container.
        /// Prefer this kind of injection over any other.
        /// </summary>
        public LoggerDefault(ICultureInfoFormatter cultureInfoFormatter)
        {
            this.cultureInfoFormatter = cultureInfoFormatter;
        }

        void ILogger.LogInfo(string messsage)
        {
            UnityEngine.Debug.Log($"{messsage}, current formatter is '{cultureInfoFormatter.GetType().Name}'");
        }
    }
}