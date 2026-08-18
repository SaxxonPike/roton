using System;
using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IFlags : ICollection<string>
{
    string this[int index] { get; set; }
    void Add(ReadOnlySpan<char> item);
    bool Contains(ReadOnlySpan<char> item);
    bool Remove(ReadOnlySpan<char> item);
}