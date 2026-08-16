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
#if NET10_0_OR_GREATER
    private readonly Dictionary<string, ICheat>.AlternateLookup<ReadOnlySpan<char>> _cheats;
#else
    private readonly Dictionary<string, ICheat> _cheats;
#endif

    public CheatList(
        IContextMetadataService contextMetadataService,
        IEnumerable<ICheat> cheats)
    {
        var result = new Dictionary<string, ICheat>(StringComparer.OrdinalIgnoreCase);

        foreach (var cheat in cheats)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(cheat))
                result.Add(attribute.Name, cheat);
        }

#if NET10_0_OR_GREATER
        _cheats = result.GetAlternateLookup<ReadOnlySpan<char>>();
#else
        _cheats = result;
#endif
    }

    public ICheat Get(ReadOnlySpan<char> name)
    {
#if NET10_0_OR_GREATER
        return _cheats.TryGetValue(name, out var value) ? value : null;
#else
        foreach (var entry in _cheats)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
#endif
    }
}