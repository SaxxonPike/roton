using System;

namespace Roton.Emulation.Data;

/// <summary>
/// Represents a timer.
/// </summary>
public interface ITimer
{
    /// <summary>
    /// Gets or sets the high-precision representation of the timer's last update.
    /// </summary>
    TimeSpan LastUpdate { get; set; }

    /// <summary>
    /// Gets a reference to the value that represents the elapsed time
    /// of the timer in hundredths of a second.
    /// </summary>
    ref Word Value { get; }

    /// <summary>
    /// Updates the timer value using tick-based timing.
    /// Use this when synchronization needs to be tick-exact.
    /// </summary>
    /// <remarks>
    /// RoZ: SoundHasTimeElapsed
    /// </remarks>
    bool UpdateByGameTicks(TimeSpan totalElapsed, int intervalGameTicks);

    /// <summary>
    /// Updates the timer value using exact hundredths of a second.
    /// Use this when the timer needs to be synchronized with real time.
    /// </summary>
    /// <remarks>
    /// RoZ: SoundHasTimeElapsed
    /// </remarks>
    bool UpdateByRealTime(TimeSpan totalElapsed, int intervalHsec);

    /// <summary>
    /// Resets the timer to zero.
    /// </summary>
    void Reset();
}