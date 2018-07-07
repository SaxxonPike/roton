using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Directions;

namespace Roton.Emulation.SuperZZT
{
    public abstract class Directions : IDirections
    {
        private readonly IDictionary<string, IDirection> _commands;

        protected Directions(IEnumerable<IDirection> commands, string[] enabledNames)
        {
            _commands = commands
                .Where(c => enabledNames.Contains(c.Name))
                .ToDictionary(c => c.Name, c => c);
        }
        
        public IDirection Get(string name)
        {
            return _commands.ContainsKey(name)
                ? _commands[name]
                : null;
        }        
    }
}