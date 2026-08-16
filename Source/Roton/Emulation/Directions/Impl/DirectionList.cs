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
    private readonly Lazy<IDictionary<string, IDirection>> _directions;
    private IDictionary<string, IDirection> Directions => _directions.Value;

    public DirectionList(IContextMetadataService contextMetadataService,
        Lazy<IEnumerable<IDirection>> directions)
    {
        _directions = new Lazy<IDictionary<string, IDirection>>(() =>
        {
            var result = new Dictionary<string, IDirection>();

            foreach (var direction in directions.Value)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(direction))
                    result.Add(attribute.Name, direction);
            }

            return result;
        });
    }

    public IDirection Get(ReadOnlySpan<char> name)
    {
        foreach (var entry in Directions)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
    }
}