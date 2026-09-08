namespace Roton;

/// <summary>
/// Indicates a particular engine.
/// </summary>
public enum Context
{
    /// <summary>
    /// Indicates that the Original engine applies.
    /// </summary>
    Original = -1,

    /// <summary>
    /// Indicates that the Super engine applies.
    /// </summary>
    Super = -2,

    /// <summary>
    /// Indicates the editor's non-game-specific context.
    /// </summary>
    Editor = 0x10000
}