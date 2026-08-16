using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class TargetList : ITargetList
{
    private readonly Dictionary<string, ITarget> _targets;

    public TargetList(IContextMetadataService contextMetadataService,
        IEnumerable<ITarget> targets)
    {
        var result = new Dictionary<string, ITarget>();

        foreach (var target in targets)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(target))
                result.Add(attribute.Name, target);
        }

        _targets = result;
    }

    public ITarget Get(ReadOnlySpan<char> name)
    {
        foreach (var entry in _targets)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
    }
}