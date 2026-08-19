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
#if NET10_0_OR_GREATER
    private readonly Dictionary<string, ITarget>.AlternateLookup<ReadOnlySpan<char>> _targets;
#else
    private readonly Dictionary<string, ITarget> _targets;
#endif

    public TargetList(IContextMetadataService contextMetadataService,
        IEnumerable<ITarget> targets)
    {
        var result = new Dictionary<string, ITarget>(StringComparer.OrdinalIgnoreCase);

        foreach (var target in targets)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(target))
                result.Add(attribute.Name, target);
        }

#if NET10_0_OR_GREATER
        _targets = result.GetAlternateLookup<ReadOnlySpan<char>>();
#else
        _targets = result;
#endif
    }

    public ITarget? Get(ReadOnlySpan<char> name)
    {
#if NET10_0_OR_GREATER
        return _targets.TryGetValue(name, out var value) ? value : null;
#else
        foreach (var entry in _targets)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
#endif
    }
}