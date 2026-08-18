using System;
using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IColors : IEnumerable<string>
{
    string this[int index] { get; }
    int IndexOf(ReadOnlySpan<char> item);
}