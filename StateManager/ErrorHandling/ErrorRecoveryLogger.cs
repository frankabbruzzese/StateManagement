using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace StateManager.ErrorHandling
{
    /// <summary>
    /// internal use
    /// </summary>
    public class ErrorRecoveryLogger : ILogger
    {
        IErrorHandler errorHendler;
        public ErrorRecoveryLogger(IErrorHandler errorHendler)
        {
            this.errorHendler = errorHendler;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return new FakeScope();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Error;
        }

        public void Log<TState>(LogLevel logLevel, EventId 
            eventId, TState state, Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            if (logLevel < LogLevel.Error) return;
            errorHendler.Trigger(exception);
        }
    }
    /// <summary>
    /// internal use
    /// </summary>
    public class FakeScope : IDisposable
    {
        public void Dispose()
        {
            
        }
    }
}
