using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Directions
{
    public abstract class DirectionList : IDirectionList
    {
        private readonly IDictionary<string, IDirection> _commands;

        protected DirectionList(IDictionary<string, IDirection> commands)
        {
            _commands = commands;
        }
        
        public IDirection Get(string name)
        {
            return _commands.ContainsKey(name)
                ? _commands[name]
                : null;
        }        
    }
}