using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

public abstract class TypeList<T>(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    where T : class
{
    private bool _initialized;

    private T[] _itemsById = [];
    private int _minId;
    private int _maxId;

    private T? _defaultItemById;
    private T? _defaultItemByName;

#if NET10_0_OR_GREATER
    private Dictionary<string, T>.AlternateLookup<ReadOnlySpan<char>> _items;
#else
    private Dictionary<string, T>? _items = [];
#endif

    private void Initialize()
    {
        _initialized = true;

        var result = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        var resultById = new Dictionary<int, T>();
        var candidates = (serviceProvider.GetService(typeof(IEnumerable<T>)) as IEnumerable<T> ?? [])
            .SelectMany(x => contextMetadataService.GetMetadata(x)
                .Select(y => (Item: x, Metadata: y)))
            .OrderBy(x => x.Metadata.Id)
            .ThenBy(x => x.Metadata.Name)
            .ToList();

        _minId = candidates.DefaultIfEmpty().Min(x => x.Metadata.Id);
        _maxId = candidates.DefaultIfEmpty().Max(x => x.Metadata.Id);

        foreach (var (item, attribute) in candidates)
        {
            if (!string.IsNullOrEmpty(attribute.Name))
                result.Add(attribute.Name.ToUpper(), item);
            else if (_defaultItemByName == null)
                _defaultItemByName = item;

            if (attribute.Id >= 0)
                resultById.Add(attribute.Id, item);
            else if (_defaultItemById == null)
                _defaultItemById = item;
        }

        _itemsById = new T[_maxId - _minId + 1];
        foreach (var kv in resultById)
            _itemsById[kv.Key - _minId] = kv.Value;

#if NET10_0_OR_GREATER
        _items = result.GetAlternateLookup<ReadOnlySpan<char>>();
#else
        _items = result;
#endif
    }

    public T? Get(int id)
    {
        if (!_initialized)
            Initialize();

        if (id < _minId || id > _maxId)
            return _defaultItemById;
        return _itemsById[id - _minId];
    }

    public T? Get(ReadOnlySpan<char> name)
    {
        if (!_initialized)
            Initialize();

#if NET10_0_OR_GREATER
        return _items.TryGetValue(name, out var value) ? value : _defaultItemByName;
#else
        foreach (var entry in _items!)
        {
            if (name.SequenceEqual(entry.Key))
                return entry.Value;
        }

        return _defaultItemByName;
#endif
    }
}