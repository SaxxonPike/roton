using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Conditions;

namespace Roton.Emulation.SuperZZT
{
    public abstract class Conditions : IConditions
    {
        private readonly IDictionary<string, ICondition> _commands;

        protected Conditions(IEnumerable<ICondition> commands, string[] enabledNames)
        {
            _commands = commands
                .Where(c => enabledNames.Contains(c.Name))
                .ToDictionary(c => c.Name, c => c);
        }
        
        public ICondition Get(string name)
        {
            return _commands.ContainsKey(name)
                ? _commands[name]
                : null;
        }        
    }
}