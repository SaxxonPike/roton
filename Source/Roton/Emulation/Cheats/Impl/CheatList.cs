using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class CheatList : ICheatList
{
    private readonly Lazy<IDictionary<string, ICheat>> _cheats;
    private IDictionary<string, ICheat> Cheats => _cheats.Value;

    public CheatList(IContextMetadataService contextMetadataService, 
        Lazy<IEnumerable<ICheat>> cheats)
    {
        _cheats = new Lazy<IDictionary<string, ICheat>>(() =>
        {
            var result = new Dictionary<string, ICheat>();

            foreach (var cheat in cheats.Value)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(cheat))
                    result.Add(attribute.Name, cheat);
            }

            return result;
        });
    }

    public ICheat Get(ReadOnlySpan<char> name)
    {
        foreach (var entry in Cheats)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
    }
}