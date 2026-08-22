using System;
using System.Collections.Generic;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

public abstract class TypeListById<T>(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    where T : class
{
    private bool _initialized;

    private T[] _items = [];
    private int _minId;
    private int _maxId;

    public T? Get(int index)
    {
        if (!_initialized)
        {
            _initialized = true;

            var result = new Dictionary<int, T>();

            foreach (var item in serviceProvider.GetService(typeof(IEnumerable<T>)) as IEnumerable<T> ?? [])
            {
                foreach (var attribute in contextMetadataService.GetMetadata(item))
                {
                    result.Add(attribute.Id, item);

                    if (attribute.Id < _minId)
                        _minId = attribute.Id;
                    else if (attribute.Id > _maxId)
                        _maxId = attribute.Id;
                }
            }

            _items = new T[_maxId - _minId + 1];
            foreach (var kv in result)
                _items[kv.Key - _minId] = kv.Value;
        }

        if (index < _minId || index > _maxId)
            return null;
        return _items[index - _minId];
    }
}