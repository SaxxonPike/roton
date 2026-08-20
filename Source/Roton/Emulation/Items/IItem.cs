using Roton.Emulation.Data;

namespace Roton.Emulation.Items;

public interface IItem
{
    ref Word Value { get; }
}