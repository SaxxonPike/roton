namespace Roton.Emulation.Interactions;

/// <summary>
/// Resolves player interactions for elements.
/// </summary>
public interface IInteractionList
{
    /// <summary>
    /// Gets the player interaction for the specified element ID.
    /// </summary>
    /// <param name="elementId">
    /// ID of the element to get the interaction for.
    /// </param>
    /// <returns>
    /// The interaction handler for the specified element ID, or null if no interaction is defined.
    /// </returns>
    IInteraction? Get(int elementId);
}