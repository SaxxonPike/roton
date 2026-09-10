namespace Roton.Emulation.Core;

/// <summary>
/// Handles conversion of time units.
/// </summary>
public interface ITimeConverter
{
    /// <summary>
    /// Converts hundredths of seconds to ticks.
    /// </summary>
    /// <param name="hsec">
    /// Duration in hundredths of seconds.
    /// </param>
    /// <returns>
    /// The equivalent number of ticks.
    /// </returns>
    int HsecToTicks(int hsec);
}