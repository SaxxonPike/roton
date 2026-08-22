using System;
using System.Collections.Generic;
#if NET10_0_OR_GREATER
using System.Threading;
#endif
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

public abstract class TypeListById<T>(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    where T : class
{
    private bool _initialized;

#if NET10_0_OR_GREATER
    private readonly Lock _dictLock = new();
#else
    private readonly object _dictLock = new();
#endif

    private T[] _items = [];
    private int _minId;
    private int _maxId;

    public T? Get(int index)
    {
        //lock (_dictLock)
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
}