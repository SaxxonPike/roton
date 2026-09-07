namespace Roton.Emulation.Cheats;

/// <summary>
/// Represents a cheat code that can be executed in-game.
/// </summary>
public interface ICheat
{
    /// <summary>
    /// Executes the cheat code.
    /// </summary>
    /// <param name="clear">
    /// If true, the entered code was preceded with a hyphen.
    /// </param>
    /// <remarks>
    /// RoZ: GameDebugPrompt (execution only)
    /// </remarks>
    void Execute(bool clear);
}