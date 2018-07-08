using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Conditions
{
    public abstract class ConditionList : IConditionList
    {
        private readonly IDictionary<string, ICondition> _commands;

        protected ConditionList(IDictionary<string, ICondition> conditions)
        {
            _commands = conditions;
        }
        
        public ICondition Get(string name)
        {
            return _commands.ContainsKey(name)
                ? _commands[name]
                : null;
        }        
    }
}