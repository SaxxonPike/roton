using System;
using System.Collections.Generic;
using System.IO;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using static System.Buffers.Binary.BinaryPrimitives;

namespace Roton.Editors;

public struct Coord
{
    public int X { get; set; }
    public int Y { get; set; }
}

public struct Tile
{
    public int Id { get; set; }
    public int Color { get; set; }
}

public class Editor(IServiceProvider serviceProvider)
{
    public WorldEditor? World { get; private set; }

    internal T GetService<T>() => (T)serviceProvider.GetService(typeof(T)) ??
                                  throw new RotonException("Required service not found: " + typeof(T).Name);

    public void Load(Stream stream)
    {
        var ser = GetService<IWorldImporter>();
        var inWorld = GetService<IWorld>();
        var inBoard = GetService<IBoard>();
        var packedBoards = GetService<IBoardList>();
        var packer = GetService<IBoardPacker>();

        ser.ImportWorld(stream);

        World = new WorldEditor(this)
        {
            WorldType = inWorld.WorldType,
            Name = inWorld.Name,
            Ammo = inWorld.Ammo,
            StartBoard = inWorld.BoardIndex,
            EnergyCycles = inWorld.EnergyCycles,
            Gems = inWorld.Gems,
            Health = inWorld.Health,
            IsLocked = inWorld.IsLocked,
            Score = inWorld.Score,
            Stones = inWorld.Stones,
            TimePassed = TimeSpan.FromSeconds(inWorld.TimePassed) +
                         TimeSpan.FromMilliseconds((int)inWorld.TimeLimitTimer.Value * 10),
            TorchCycles = inWorld.TorchCycles,
            Torches = inWorld.Torches,
            Flags = [],
            Boards = []
        };

        foreach (var flag in inWorld.Flags)
            World.Flags.Add(flag);

        foreach (var board in packedBoards)
        {
            try
            {

            }
            catch
            {
                
            }
            packer.Unpack(board.Data);
            
        }
    }
}

public class WorldEditor(Editor editor)
{
    public int WorldType { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Ammo { get; set; }
    public int StartBoard { get; set; }
    public int EnergyCycles { get; set; }
    public int Gems { get; set; }
    public int Health { get; set; }
    public bool IsLocked { get; set; }
    public int Score { get; set; }
    public int Stones { get; set; }
    public TimeSpan TimePassed { get; set; }
    public int TorchCycles { get; set; }
    public int Torches { get; set; }

    public byte[] Reserved { get; set; } = [];

    public List<string> Flags { get; set; } = [];
    public List<BoardEditor> Boards { get; set; } = [];
}

public class BoardEditor(Editor editor)
{
    public string Name { get; set; } = string.Empty;
    public Coord Camera { get; set; }
    public Coord Entrance { get; set; }
    public bool IsDark { get; set; }
    public int MaxShots { get; set; }
    public bool RestartOnZap { get; set; }
    public TimeSpan TimeLimit { get; set; }
    public Tile[,] Tiles { get; set; } = new Tile[0, 0];

    public byte[] Reserved { get; set; } = [];

    public List<ActorEditor> Actors { get; set; } = [];
}

public class ActorEditor(Editor editor)
{
    public Coord Location { get; set; }
    public Tile UnderTile { get; set; }
    public Coord Vector { get; set; }
    public int Cycle { get; set; }
    public int Follower { get; set; }
    public int Leader { get; set; }
    public int Instruction { get; set; }
    public int Length { get; set; }
    public int P1 { get; set; }
    public int P2 { get; set; }
    public int P3 { get; set; }
    public int Pointer { get; set; }
    public string Code { get; set; } = string.Empty;

    public byte[] Reserved { get; set; } = [];
}