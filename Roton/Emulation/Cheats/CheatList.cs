using System.Collections.Generic;

namespace Roton.Emulation.Cheats
{
    public abstract class CheatList : ICheatList
    {
        private readonly IDictionary<string, ICheat> _cheats;

        protected CheatList(IDictionary<string, ICheat> cheats)
        {
            _cheats = cheats;
        }
        
        public ICheat Get(string name)
        {
            return _cheats.ContainsKey(name)
                ? _cheats[name]
                : null;
        }
    }
}