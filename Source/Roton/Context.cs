namespace Roton;

/// <summary>
/// Indicates a particular engine.
/// </summary>
public enum Context
{
    /// <summary>
    /// Indicates that all engines apply. Services marked with this context are
    /// available to all engines and should automatically be activated.
    /// </summary>
    Startup = 1,

    /// <summary>
    /// Indicates that the Original engine applies.
    /// </summary>
    Original,

    /// <summary>
    /// Indicates that the Super engine applies.
    /// </summary>
    Super
}