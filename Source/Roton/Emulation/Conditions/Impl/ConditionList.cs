using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class ConditionList : IConditionList
{
#if NET10_0_OR_GREATER
    private readonly Dictionary<string, ICondition>.AlternateLookup<ReadOnlySpan<char>> _conditions;
#else
    private readonly Dictionary<string, ICondition> _conditions;
#endif

    public ConditionList(IContextMetadataService contextMetadataService,
        IEnumerable<ICondition> conditions)
    {
        var result = new Dictionary<string, ICondition>(StringComparer.OrdinalIgnoreCase);

        foreach (var condition in conditions)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(condition))
                result.Add(attribute.Name, condition);
        }

#if NET10_0_OR_GREATER
        _conditions = result.GetAlternateLookup<ReadOnlySpan<char>>();
#else
        _conditions = result;
#endif
    }

    public ICondition Get(ReadOnlySpan<char> name)
    {
#if NET10_0_OR_GREATER
        return _conditions.TryGetValue(name, out var value) ? value : null;
#else
        foreach (var entry in _conditions)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
#endif
    }
}