using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace StateManager.ErrorHandling
{
    /// <summary>
    /// internal use
    /// </summary>
    public class ErrorRecoveryLogProvider : ILoggerProvider
    {
        IErrorHandler errorHandler;
        public ErrorRecoveryLogProvider(IErrorHandler handler)
        {
            errorHandler = handler;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new ErrorRecoveryLogger(errorHandler);
        }

        public void Dispose()
        {
            
        }
    }
}
