using Roton.Emulation.Data;

namespace Roton.Emulation.Interactions;

/// <summary>
/// Represents a player interaction handler for an element.
/// </summary>
public interface IInteraction
{
    /// <summary>
    /// Handles the player interaction for the specified location.
    /// </summary>
    /// <param name="location">
    /// The location of the element being interacted with.
    /// </param>
    /// <param name="index">
    /// Index of the actor that initiated the interaction.
    /// </param>
    /// <param name="vector">
    /// Interaction vector, which may be modified.
    /// </param>
    void Interact(Location location, int index, ref Vector vector);
}