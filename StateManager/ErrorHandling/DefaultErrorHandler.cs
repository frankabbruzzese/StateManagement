using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace StateManager.ErrorHandling
{
    public class DefaultErrorHandler: IErrorHandler
    {
        public event Func<Exception, Task> OnException;
        public async Task Trigger(Exception ex)
        {
            if (OnException != null) await OnException(ex);
        }
    }
}
