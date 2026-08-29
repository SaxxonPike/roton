using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Targets;

public interface ITarget
{
    bool Execute(int index, ref SearchContext context, ReadOnlySpan<char> term);
}