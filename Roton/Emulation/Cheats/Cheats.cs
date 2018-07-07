using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Cheats;

namespace Roton.Emulation.SuperZZT
{
    public abstract class Cheats : ICheats
    {
        private readonly IDictionary<string, ICheat> _cheats;

        protected Cheats(IEnumerable<ICheat> cheats, string[] enabledNames)
        {
            _cheats = cheats
                .Where(c => enabledNames.Contains(c.Name))
                .ToDictionary(c => c.Name, c => c);
        }
        
        public ICheat Get(string name)
        {
            return _cheats.ContainsKey(name)
                ? _cheats[name]
                : null;
        }
    }
}