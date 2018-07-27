using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class CheatList : ICheatList
    {
        private readonly Lazy<IDictionary<string, ICheat>> _cheats;

        public CheatList(Lazy<IContextMetadataService> contextMetadataService, Lazy<IEnumerable<ICheat>> cheats)
        {
            _cheats = new Lazy<IDictionary<string, ICheat>>(() =>
            {
                var result = new Dictionary<string, ICheat>();

                foreach (var cheat in cheats.Value)
                {
                    foreach (var attribute in contextMetadataService.Value.GetMetadata(cheat))
                        result.Add(attribute.Name, cheat);
                }

                return result;
            });
        }

        private IDictionary<string, ICheat> Cheats => _cheats.Value;

        public ICheat Get(string name)
        {
            return Cheats.ContainsKey(name)
                ? Cheats[name]
                : null;
        }
    }
}