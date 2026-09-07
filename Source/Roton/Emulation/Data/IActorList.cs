using System;
using System.Collections.Generic;

namespace Roton.Emulation.Data;

/// <summary>
/// Represents a list of actors.
/// </summary>
public interface IActorList : IEnumerable<IActor>
{
    /// <summary>
    /// RoZ: MAX_STAT+2
    /// </summary>
    int Capacity { get; }

    /// <summary>
    /// RoZ: Board.StatCount
    /// </summary>
    int Count { get; }

    /// <remarks>
    /// RoZ: Board.Stats
    /// </remarks>
    IActor this[int index] { get; }

    /// <remarks>
    /// RoZ: Board.Stats[0]
    /// </remarks>
    IActor Player { get; }

    /// <remarks>
    /// RoZ: GetStatIdAt
    /// </remarks>
    int IndexAt(Location location);

    /// <remarks>
    /// RoZ: Board.Stats[index].Data
    /// </remarks>
    Span<char> GetCode(int index);
}