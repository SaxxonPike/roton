using System;

namespace Roton.Emulation.Items;

/// <summary>
/// Resolves item quantities by name.
/// </summary>
public interface IItemList
{
    /// <summary>
    /// Gets the item quantity with the specified name.
    /// </summary>
    /// <param name="name">
    /// Name of the item quantity.
    /// </param>
    /// <returns>
    /// Item quantity with the specified name, or null if no item quantity is defined.
    /// </returns>
    IItem? Get(ReadOnlySpan<char> name);
}