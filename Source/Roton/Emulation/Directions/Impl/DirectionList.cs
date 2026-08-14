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

    public DirectionList(Lazy<IContextMetadataService> contextMetadataService,
        Lazy<IEnumerable<IDirection>> directions)
    {
        _directions = new Lazy<IDictionary<string, IDirection>>(() =>
        {
            var result = new Dictionary<string, IDirection>();
            foreach (var direction in directions.Value)
            {
                foreach (var attribute in contextMetadataService.Value.GetMetadata(direction))
                    result.Add(attribute.Name, direction);
            }

            return result;
        });
    }

    public IDirection Get(string name)
    {
        return _directions.Value.ContainsKey(name)
            ? _directions.Value[name]
            : null;
    }
}