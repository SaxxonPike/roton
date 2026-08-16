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
    private readonly Lazy<IDictionary<string, ICondition>> _conditions;
    private IDictionary<string, ICondition> Conditions => _conditions.Value;

    public ConditionList(Lazy<IContextMetadataService> contextMetadataService,
        Lazy<IEnumerable<ICondition>> conditions)
    {
        _conditions = new Lazy<IDictionary<string, ICondition>>(() =>
        {
            var result = new Dictionary<string, ICondition>();

            foreach (var condition in conditions.Value)
            {
                foreach (var attribute in contextMetadataService.Value.GetMetadata(condition))
                    result.Add(attribute.Name, condition);
            }

            return result;
        });
    }

    public ICondition Get(ReadOnlySpan<char> name)
    {
        foreach (var entry in Conditions)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
    }
}