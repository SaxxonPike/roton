using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IBroadcaster
{
    bool BroadcastLabel(int sender, ReadOnlySpan<char> label, bool ignoreLock);
    bool ExecuteLabel(int sender, ref SearchContext search, ReadOnlySpan<char> term, ReadOnlySpan<char> prefix);
}