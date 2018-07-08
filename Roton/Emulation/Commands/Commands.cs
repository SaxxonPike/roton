using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Commands
{
    public abstract class Commands : ICommands
    {
        private readonly IDictionary<string, ICommand> _commands;

        protected Commands(IEnumerable<ICommand> commands, string[] enabledNames)
        {
            _commands = commands
                .Where(c => enabledNames.Contains(c.Name))
                .ToDictionary(c => c.Name, c => c);
        }
        
        public ICommand Get(string name)
        {
            return _commands.ContainsKey(name)
                ? _commands[name]
                : null;
        }        
    }
}