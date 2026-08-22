using System;
using System.Collections.Generic;
#if NET10_0_OR_GREATER
using System.Threading;
#endif
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

public abstract class TypeListByName<T>(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    where T : class
{
    private bool _initialized;

#if NET10_0_OR_GREATER
    private readonly Lock _dictLock = new();
    private Dictionary<string, T>.AlternateLookup<ReadOnlySpan<char>> _items;
#else
    private readonly object _dictLock = new();
    private Dictionary<string, T>? _items = [];
#endif

    public T? Get(ReadOnlySpan<char> name)
    {
        //lock (_dictLock)
        {

            if (!_initialized)
            {
                _initialized = true;

                var result = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

                foreach (var item in serviceProvider.GetService(typeof(IEnumerable<T>)) as IEnumerable<T> ?? [])
                {
                    foreach (var attribute in contextMetadataService.GetMetadata(item!))
                        result.Add(attribute.Name, item);
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
                if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                    return entry.Value;
            }

            return null;
#endif
        }
    }
}