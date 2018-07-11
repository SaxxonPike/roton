using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class CheatList : ICheatList
    {
        private readonly IDictionary<string, ICheat> _cheats = new Dictionary<string, ICheat>();

        public CheatList(IContextMetadataService contextMetadataService, IEnumerable<ICheat> cheats)
        {
            foreach (var cheat in cheats)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(cheat))
                    _cheats.Add(attribute.Name, cheat);
            }
        }
        
        public ICheat Get(string name)
        {
            return _cheats.ContainsKey(name)
                ? _cheats[name]
                : null;
        }        
    }
}