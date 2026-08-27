using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IRefList<T> : IEnumerable<T>
{
    int Count { get; }
    ref T this[int index] { get; }
}