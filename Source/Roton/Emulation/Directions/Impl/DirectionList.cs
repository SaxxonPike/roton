using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class DirectionList : IDirectionList
{
    private readonly Dictionary<string, IDirection> _directions;

    public DirectionList(IContextMetadataService contextMetadataService,
        IEnumerable<IDirection> directions)
    {
        var result = new Dictionary<string, IDirection>();

        foreach (var direction in directions)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(direction))
                result.Add(attribute.Name, direction);
        }

        _directions = result;
    }

    public IDirection Get(ReadOnlySpan<char> name)
    {
        foreach (var entry in _directions)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
    }
}