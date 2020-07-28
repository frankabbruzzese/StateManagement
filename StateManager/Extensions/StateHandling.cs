
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using StateManager.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StateManager.Extensions
{
    /// <summary>
    /// static class that contains
    /// all extension methods that register
    /// state management, global error handling
    /// and application closure automatic actions
    /// </summary>
    public static class StateHandling
    {
        
        internal static ILoggingBuilder 
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
            services.AddSingleton<TasksStateService>();
            return services;
        }
        
        public static async Task<IServiceProvider> EnableUnloadEvents(this IServiceProvider services)
        {
            var state = services.GetRequiredService<TasksStateService>();
            IJSRuntime jSRuntime = services.GetRequiredService<IJSRuntime>();
            await jSRuntime.InvokeVoidAsync(
                    "stateManager.AddUnloadListeners", state.JsTeference);
            return services;
        }
    }
}
