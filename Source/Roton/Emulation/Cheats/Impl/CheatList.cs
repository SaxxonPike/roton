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
    private readonly Dictionary<string, ICheat> _cheats;

    public CheatList(
        IContextMetadataService contextMetadataService,
        IEnumerable<ICheat> cheats)
    {
        var result = new Dictionary<string, ICheat>();

        foreach (var cheat in cheats)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(cheat))
                result.Add(attribute.Name, cheat);
        }

        _cheats = result;
    }

    public ICheat Get(ReadOnlySpan<char> name)
    {
        foreach (var entry in _cheats)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
    }
}