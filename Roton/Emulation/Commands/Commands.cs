using System.Collections.Generic;

namespace Roton.Emulation.Commands
{
    public abstract class Commands : ICommands
    {
        private readonly IDictionary<string, ICommand> _commands;

        protected Commands(IDictionary<string, ICommand> commands)
        {
            _commands = commands;
        }
        
        public ICommand Get(string name)
        {
            return _commands.ContainsKey(name)
                ? _commands[name]
                : null;
        }        
    }
}