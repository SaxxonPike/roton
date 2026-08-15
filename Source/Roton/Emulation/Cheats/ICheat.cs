using System;

namespace Roton.Emulation.Cheats;

public interface ICheat
{
    void Execute(ReadOnlySpan<char> name, bool clear);
}