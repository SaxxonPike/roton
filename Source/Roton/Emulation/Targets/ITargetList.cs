using System;

namespace Roton.Emulation.Targets;

public interface ITargetList
{
    ITarget Get(ReadOnlySpan<char> name);
}