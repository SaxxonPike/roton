using System;
using System.Collections.Generic;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class DirectionList : IDirectionList
{
#if NET10_0_OR_GREATER
    private readonly Dictionary<string, IDirection>.AlternateLookup<ReadOnlySpan<char>> _directions;
#else
    private readonly Dictionary<string, IDirection> _directions;
#endif

    public DirectionList(IContextMetadataService contextMetadataService,
        IEnumerable<IDirection> directions)
    {
        var result = new Dictionary<string, IDirection>(StringComparer.OrdinalIgnoreCase);

        foreach (var direction in directions)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(direction))
                result.Add(attribute.Name, direction);
        }

#if NET10_0_OR_GREATER
        _directions = result.GetAlternateLookup<ReadOnlySpan<char>>();
#else
        _directions = result;
#endif
    }

    public IDirection? Get(ReadOnlySpan<char> name)
    {
#if NET10_0_OR_GREATER
        return _directions.TryGetValue(name, out var value) ? value : null;
#else
        foreach (var entry in _directions)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
#endif
    }
}