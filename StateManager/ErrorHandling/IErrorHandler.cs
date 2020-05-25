using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StateManager.ErrorHandling
{
    public interface IErrorHandler
    {
        event Func<Exception, Task> OnException;
        Task Trigger(Exception ex);
        
    }
}
