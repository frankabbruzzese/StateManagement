using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Threading.Tasks;
using StateManager.ErrorHandling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace StateManager
{
    public class TasksStateService: IDisposable
    {
        private Dictionary<string, StateContainer> 
            OverallState
            = new Dictionary<string, StateContainer>();
        
        IJSRuntime JSRuntime;
        IErrorHandler ErrorHandler;
        public TasksStateService(IJSRuntime jSRuntime,
            IErrorHandler errorHandler)
        {
            JSRuntime = jSRuntime;
            ErrorHandler = errorHandler;
            ErrorHandler.OnException += SaveError;
        }

        public bool IsRunning(string x)
        {
            return OverallState.TryGetValue(x, out var state)
                && state.IsRunning;
        }
        public bool IsDirty()
        {
            return OverallState.Values
                .Any(x => x.IsRunning);
        }
        public T Get<T>(string x, bool createNew)
        {
            if (OverallState.TryGetValue(x, out var state))
            {
                if (state.IsRunning) return (T)state.RoughState;
                else if (createNew)
                {
                    var res = Activator.CreateInstance<T>();
                    ((StateContainer<T>)state).State = res;
                    return res;
                }
                else return default;
            }
            else if (createNew)
            {
                var container = new StateContainer<T>
                {
                    State = Activator.CreateInstance<T>()
                };
                OverallState[x] = container;
                return container.State;
            }
            else return default;
        }
        public void Finish<T>(string x)
        {
            if (OverallState.TryGetValue(x, out var state))
                ((StateContainer<T>)state).State = default;
        }
        protected virtual string Serialize()
        {
            var toSerialize=OverallState
                .Where(m => m.Value.IsRunning)
                .ToList();
            string result=null;
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                
                formatter.Serialize(stream, toSerialize);
                
                stream.Flush();
                result=Convert.ToBase64String(stream.ToArray());
            }
            return result;
        }
        protected virtual void Deserialize(string s)
        {
            byte[] binary = Convert.FromBase64String(s);
            using (var stream = new MemoryStream(binary))
            {
                var formatter = new BinaryFormatter();
                var res = formatter.Deserialize(stream) 
                    as List<KeyValuePair<string, StateContainer>>;
                OverallState = res.ToDictionary(m => m.Key, m => m.Value);
            }
        }
        public async Task Save(string key)
        {
            
            try
            {
                var s = Serialize();
                await JSRuntime
                    .InvokeVoidAsync("window.localStorage.setItem", key, s);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public string ErrorKey { get; set; }
        public async Task SaveError(Exception ex)
        {
            await Save(ErrorKey);
        }
        public async Task<bool> Load(string key)
        {
            try
            {
                var s = await JSRuntime
                     .InvokeAsync<string>("window.localStorage.getItem", key);
                if (s != null)
                {
                    Deserialize(s);
                    return true;
                }
                else return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
                
        }
        public async Task Delete(string key)
        {
            await JSRuntime
                .InvokeVoidAsync("window.localStorage.removeItem", key);
        }

        public void Dispose()
        {
            ErrorHandler.OnException -= SaveError; ;
        }
    }
}
