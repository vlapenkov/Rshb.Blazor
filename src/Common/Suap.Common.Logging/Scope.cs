using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Common.Logging
{
    public class Scope : IDisposable
    {
        protected readonly IDisposable BaseScope;
        protected readonly ILogger Logger;
        protected readonly string OperationName;
        protected readonly Stopwatch Timer;

        public Scope(ILogger logger, string operationName, object scope)
        {
            Timer = Stopwatch.StartNew();
            var scopeProps = new Dictionary<string, object>
            {
                { "scopeId", Guid.NewGuid().ToString() }
            };

            if (scope != null)
                foreach (var p in scope.GetType().GetProperties())
                    scopeProps.Add(p.Name, p.GetValue(scope));

            string messageFormat = string.Join(", ", scopeProps.Select(n => $"{n.Key} = {{{n.Key}}}"));
            object[] parameters = scopeProps.Select(p => p.Value).ToArray();
            BaseScope = logger.BeginScope(messageFormat, parameters);
            Logger = logger;
            OperationName = operationName;
            Logger.LogInformation(OperationName + ". Начало");
        }

        public void Dispose()
        {
            Timer.Stop();
            Logger.LogInformation(OperationName + ". Конец. Время: {scopeTime} мс", Timer.ElapsedMilliseconds);
            BaseScope.Dispose();
        }
    }
}