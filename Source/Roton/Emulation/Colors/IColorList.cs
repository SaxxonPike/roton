using System;

namespace Roton.Emulation.Colors;

public interface IColorList
{
    IColor? Get(ReadOnlySpan<char> name);
    IColor? Get(int id);
}