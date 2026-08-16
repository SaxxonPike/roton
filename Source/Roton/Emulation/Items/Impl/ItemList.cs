using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class ItemList : IItemList
{
#if NET10_0_OR_GREATER
    private readonly Dictionary<string, IItem>.AlternateLookup<ReadOnlySpan<char>> _items;
#else
    private readonly Dictionary<string, IItem> _items;
#endif

    public ItemList(
        IContextMetadataService contextMetadataService,
        IEnumerable<IItem> items)
    {
        var result = new Dictionary<string, IItem>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in items)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(item))
                result.Add(attribute.Name, item);
        }

#if NET10_0_OR_GREATER
        _items = result.GetAlternateLookup<ReadOnlySpan<char>>();
#else
        _items = result;
#endif
    }

    public IItem Get(ReadOnlySpan<char> name)
    {
#if NET10_0_OR_GREATER
        return _items.TryGetValue(name, out var value) ? value : null;
#else
        foreach (var entry in _items)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
#endif
    }
}