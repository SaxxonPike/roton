using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class DrawList : IDrawList
{
    private readonly Lazy<IDictionary<int, IDraw>> _draws;
    private IDictionary<int, IDraw> Draws => _draws.Value;

    public DrawList(Lazy<IContextMetadataService> contextMetadataService,
        Lazy<IEnumerable<IDraw>> draws)
    {
        _draws = new Lazy<IDictionary<int, IDraw>>(() =>
        {
            var result = new Dictionary<int, IDraw>();
            foreach (var draw in draws.Value)
            {
                foreach (var attribute in contextMetadataService.Value.GetMetadata(draw))
                    result.Add(attribute.Id, draw);
            }

            return result;
        });
    }

    public IDraw Get(int index)
    {
        return Draws.TryGetValue(index, out var value)
            ? value
            : Draws[-1];
    }
}