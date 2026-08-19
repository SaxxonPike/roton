using System.Collections.Generic;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class DrawList : IDrawList
{
    private readonly Dictionary<int, IDraw> _draws;

    public DrawList(IContextMetadataService contextMetadataService,
        IEnumerable<IDraw> draws)
    {
        var result = new Dictionary<int, IDraw>();
        foreach (var draw in draws)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(draw))
                result.Add(attribute.Id, draw);
        }

        _draws = result;
    }

    public IDraw Get(int index) =>
        _draws.TryGetValue(index, out var value)
            ? value
            : _draws[-1];
}