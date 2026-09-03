namespace Roton.Emulation.Actions;

/// <summary>
/// Resolves tick actions for actors.
/// </summary>
public interface IActionList
{
    /// <summary>
    /// Gets the tick action for the specified actor.
    /// </summary>
    /// <param name="index">
    /// Index of the actor to resolve.
    /// </param>
    /// <returns>
    /// The actor's tick action, or null if no matching tick action was found.
    /// </returns>
    IAction? Get(int index);
}