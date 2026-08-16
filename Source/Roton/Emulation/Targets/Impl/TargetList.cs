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
    private readonly Lazy<IDictionary<string, ITarget>> _targets;
    private IDictionary<string, ITarget> Targets => _targets.Value;

    public TargetList(IContextMetadataService contextMetadataService,
        Lazy<IEnumerable<ITarget>> targets)
    {
        _targets = new Lazy<IDictionary<string, ITarget>>(() =>
        {
            var result = new Dictionary<string, ITarget>();

            foreach (var target in targets.Value)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(target))
                    result.Add(attribute.Name, target);
            }

            return result;
        });
    }

    public ITarget Get(ReadOnlySpan<char> name)
    {
        foreach (var entry in Targets)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
    }
}