using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl;

public abstract class CachedFixedList<T>(int count) : FixedList<T>
{
    private bool _initialized;
    private readonly T[] _cache = new T[count];
    private readonly Dictionary<int, T> _cacheDict = new();

    public override int Count => count;

    protected abstract T InitItem(int index);

    protected sealed override T GetItem(int index)
    {
        if (!_initialized)
        {
            _initialized = true;
            for (var i = 0; i < count; i++)
                _cache[i] = InitItem(i);
        }

        if (index < 0 || index >= count)
            return _cache[index];

        if (_cacheDict.TryGetValue(index, out var cachedItem))
            return cachedItem;

        return _cacheDict[index] = InitItem(index);
    }
}