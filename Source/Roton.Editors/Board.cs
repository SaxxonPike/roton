using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Roton.Editors;

/// <summary>
/// Provides an interface to edit a board.
/// </summary>
/// <param name="editor">
/// Parent editor.
/// </param>
public class Board
{
    /// <summary>
    /// Name of the board.
    /// </summary>
    [MaxLength(byte.MaxValue)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Coordinates for the camera. The engine generally overwrites this
    /// during gameplay.
    /// </summary>
    public Coord16 Camera { get; set; }

    /// <summary>
    /// Coordinates for the player's entrance. The engine generally overwrites
    /// this during gameplay.
    /// </summary>
    public Coord Entrance { get; set; }

    /// <summary>
    /// If true, the board will be dark and can be lit by torches.
    /// </summary>
    public bool IsDark { get; set; }

    /// <summary>
    /// Maximum number of simultaneous bullets the player can fire.
    /// </summary>
    [Range(byte.MinValue, byte.MaxValue)]
    public int MaxShots { get; set; }

    /// <summary>
    /// If true, the player will re-enter the board when taking damage.
    /// </summary>
    public bool RestartOnZap { get; set; }

    /// <summary>
    /// Time limit on the board, precise to 1 second.
    /// </summary>
    public TimeSpan TimeLimit { get; set; }

    /// <summary>
    /// On-board tile data.
    /// </summary>
    public Tile[,] Tiles { get; set; } = new Tile[0, 0];

    /// <summary>
    /// Board data that is written after the end of the structured data.
    /// Not all game engines will support preserving this data when the
    /// board is repacked. If <see cref="IsCorrupted"/> is true, this will
    /// contain the entirety of the board data.
    /// </summary>
    public byte[] Reserved { get; set; } = [];

    /// <summary>
    /// Indicates whether this board was corrupted when it was imported.
    /// If true, only the contents of <see cref="Reserved"/> will be written.
    /// This can also be set True when writing raw board data that cannot be
    /// processed but is necessary for some hacks.
    /// </summary>
    public bool IsCorrupted { get; set; }

    /// <summary>
    /// Actors on the board.
    /// </summary>
    public List<Actor> Actors { get; set; } = [];
}