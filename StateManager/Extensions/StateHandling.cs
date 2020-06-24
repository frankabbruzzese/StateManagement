
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StateManager.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;


namespace StateManager.Extensions
{
    public static class StateHandling
    {
        public static ILoggingBuilder 
            CustomLogger(this ILoggingBuilder builder)
        {
            builder.Services
                .AddSingleton<ILoggerProvider, ErrorRecoveryLogProvider>();  
            return builder;
        }
        public static IServiceCollection AddStateManagemenet(
            this IServiceCollection services)
        {
            
            
            services.AddSingleton<IErrorHandler, DefaultErrorHandler>();
            services.AddLogging(builder => builder.CustomLogger());
            //add the line below
            services.AddSingleton<TasksStateService>();
            return services;
        }
    }
}
