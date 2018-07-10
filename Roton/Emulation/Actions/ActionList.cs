using System.Collections.Generic;

namespace Roton.Emulation.Actions
{
    public abstract class ActionList : IActionList
    {
        private readonly IDictionary<int, IAction> _actions;
        private readonly IAction _defaultAction;

        protected ActionList(IDictionary<int, IAction> actions, IAction defaultAction)
        {
            _actions = actions;
            _defaultAction = defaultAction;
        }
        
        public IAction Get(int index)
        {
            return _actions.ContainsKey(index) ? _actions[index] : _defaultAction;
        }
    }
}