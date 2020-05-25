using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateManagement.Client.Business
{
    public abstract class StateContainer
    {
        public abstract object RoughState {get;}
    }
    public class StateContainer<T> : StateContainer
    {
        public T State { get; set; }
        public override object RoughState => State;
    }
}
