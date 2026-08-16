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
    private readonly Dictionary<string, IItem> _items;

    public ItemList(
        IContextMetadataService contextMetadataService,
        IEnumerable<IItem> items)
    {
        var result = new Dictionary<string, IItem>();

        foreach (var item in items)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(item))
                result.Add(attribute.Name, item);
        }

        _items = result;
    }

    public IItem Get(ReadOnlySpan<char> name)
    {
        foreach (var entry in _items)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
    }
}