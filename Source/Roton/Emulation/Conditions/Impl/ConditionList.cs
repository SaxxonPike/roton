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
    private readonly Dictionary<string, ICondition> _conditions;

    public ConditionList(IContextMetadataService contextMetadataService,
        IEnumerable<ICondition> conditions)
    {
        var result = new Dictionary<string, ICondition>();

        foreach (var condition in conditions)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(condition))
                result.Add(attribute.Name, condition);
        }

        _conditions = result;
    }

    public ICondition Get(ReadOnlySpan<char> name)
    {
        foreach (var entry in _conditions)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
    }
}