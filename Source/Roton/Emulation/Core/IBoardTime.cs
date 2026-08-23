namespace Roton.Emulation.Core;

/// <summary>
/// Handles tracking the amount of time that has passed on a board.
/// </summary>
public interface IBoardTime
{
    /// <summary>
    /// Resets the time to zero.
    /// </summary>
    void Reset();
    
    /// <summary>
    /// Determines the number of hundredths of a second that have elapsed since the last call.
    /// </summary>
    int Elapse();
    
    /// <summary>
    /// Advances the time by one tick.
    /// </summary>
    void Advance();
}