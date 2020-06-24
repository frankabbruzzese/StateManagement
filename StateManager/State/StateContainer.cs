using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateManager
{
    [Serializable]
    public abstract class StateContainer
    {
        public abstract object RoughState {get;}
        public abstract bool IsRunning {get;}
    }
    [Serializable]
    public class StateContainer<T> : StateContainer
    {
        public T State { get; set; }
        public override bool IsRunning { get { 
                return State != null && !State.Equals(default); } }
        public override object RoughState => State;
    }
}
