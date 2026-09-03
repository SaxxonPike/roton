using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IBroadcaster
{
    /// <remarks>
    /// RoZ: OopSend
    /// </remarks>
    bool BroadcastLabel(int sender, ReadOnlySpan<char> label, bool ignoreLock);

    /// <summary>
    /// RoZ: OopFindLabel
    /// </summary>
    bool ExecuteLabel(int sender, ref SearchContext search, ReadOnlySpan<char> term, ReadOnlySpan<char> prefix);
}