using System;

namespace Roton.Emulation.Items;

public interface IItemList
{
    IItem Get(ReadOnlySpan<char> name);
}