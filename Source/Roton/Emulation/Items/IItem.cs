using Roton.Emulation.Data;

namespace Roton.Emulation.Items;

/// <summary>
/// Represents an item quantity.
/// </summary>
public interface IItem
{
    /// <summary>
    /// Gets a reference to the item quantity.
    /// </summary>
    ref Word Value { get; }
}