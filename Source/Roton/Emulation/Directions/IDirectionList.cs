using System;

namespace Roton.Emulation.Directions;

public interface IDirectionList
{
    IDirection Get(ReadOnlySpan<char> name);
}