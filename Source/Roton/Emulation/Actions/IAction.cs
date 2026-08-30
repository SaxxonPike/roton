namespace Roton.Emulation.Actions;

/// <summary>
/// Represents the tick action function for a game element and actor.
/// </summary>
public interface IAction
{
    /// <summary>
    /// Execute the tick action for a game element.
    /// </summary>
    /// <param name="index">
    /// Index of the actor to act upon.
    /// </param>
    void Act(int index);
}