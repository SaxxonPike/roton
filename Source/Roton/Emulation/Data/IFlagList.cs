using System;
using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IFlagList : ICollection<string>
{
    string this[int index] { get; set; }
    /// <remarks>
    /// RoZ: WorldSetFlag
    /// </remarks>
    void Add(ReadOnlySpan<char> item);
    
    /// <remarks>
    /// RoZ: WorldGetFlagPosition
    /// </remarks>
    bool Contains(ReadOnlySpan<char> item);
    
    /// <remarks>
    /// RoZ: WorldClearFlag
    /// </remarks>
    bool Remove(ReadOnlySpan<char> item);
}