using System;

namespace Roton.Emulation.Commands;

public interface ICommandList
{
    ICommand Get(ReadOnlySpan<char> name);
}