using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StateManager.Extensions;
using StateManager;

namespace StateManagement.Client
{
    public class Program
    {
        private  async static Task InitializeState(IServiceProvider services, 
            string errorStateKey,
            string exitConfirm)
        {
            var state=services.GetRequiredService<TasksStateService>();
            state.ErrorKey = errorStateKey;
            state.UnloadKey = errorStateKey;
            state.UnloadPrompt = exitConfirm;
            if (await state.Load(errorStateKey))
            {
                await state.Delete(errorStateKey);
            }

        }
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddStateManagemenet();
            var built = builder.Build();
            await InitializeState(built.Services, 
                "stateSaved", "There are unsaved changes. Quit anyway?");
            await built.Services.EnableUnloadEvents();
            await built.RunAsync();
        }
    }
}
