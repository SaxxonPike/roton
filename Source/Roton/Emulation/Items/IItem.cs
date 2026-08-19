using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Items;

public interface IItem
{
    ref Word Value { get; }
}