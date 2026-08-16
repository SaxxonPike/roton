using System;

namespace Roton.Emulation.Cheats;

public interface ICheatList
{
    ICheat Get(ReadOnlySpan<char> name);
}