using System;
using System.Collections.Generic;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

public abstract class TypeListByName<T>(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    where T : class
{
    private bool _initialized;

#if NET10_0_OR_GREATER
    private Dictionary<string, T>.AlternateLookup<ReadOnlySpan<char>> _items;
#else
    private Dictionary<string, T>? _items = [];
#endif

    public T? Get(ReadOnlySpan<char> name)
    {
        if (!_initialized)
        {
            _initialized = true;

            var result = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in serviceProvider.GetService(typeof(IEnumerable<T>)) as IEnumerable<T> ?? [])
            {
                foreach (var attribute in contextMetadataService.GetMetadata(item!))
                    result.Add(attribute.Name.ToUpper(), item);
            }

#if NET10_0_OR_GREATER
            _items = result.GetAlternateLookup<ReadOnlySpan<char>>();
#else
            _items = result;
#endif
        }

#if NET10_0_OR_GREATER
        return _items.TryGetValue(name, out var value) ? value : null;
#else
        foreach (var entry in _items!)
        {
            if (name.SequenceEqual(entry.Key))
                return entry.Value;
        }

        return null;
#endif
    }
}