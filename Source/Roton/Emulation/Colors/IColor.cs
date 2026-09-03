namespace Roton.Emulation.Colors;

/// <summary>
/// Represents a color as understood by the scripting engine.
/// </summary>
public interface IColor
{
    /// <summary>
    /// Display name of the color.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Value of the color.
    /// </summary>
    int Value { get; }
}