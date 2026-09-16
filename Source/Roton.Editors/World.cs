using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Buffers.Binary.BinaryPrimitives;

namespace Roton.Editors;

/// <summary>
/// Provides an interface to edit a world.
/// </summary>
public class World
{
    [Range(short.MinValue, short.MaxValue)]
    public int WorldType { get; set; }

    [MaxLength(byte.MaxValue)] public string Name { get; set; } = string.Empty;

    [Range(short.MinValue, short.MaxValue)]
    public int Ammo { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int StartBoard { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int EnergyCycles { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int Gems { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int Health { get; set; }

    public bool IsLocked { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int Score { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int Stones { get; set; }

    public TimeSpan TimePassed { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int TorchCycles { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int Torches { get; set; }

    public byte[] Reserved { get; set; } = [];

    public List<string> Flags { get; set; } = [];
    public List<Board> Boards { get; set; } = [];
}