using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.InteropServices;
using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Editors;

public class Editor(Func<Context, IServiceProvider> serviceProviderFactory)
{
    public World? World { get; private set; }

    private static T GetService<T>(IServiceProvider serviceProvider) =>
        (T)serviceProvider.GetService(typeof(T)) ??
        throw new RotonException(
            "Required service not found: " + typeof(T).Name);

    public void Load(Stream stream, int length)
    {
        // Loading worlds is an interesting process:
        // - Ensure that there is an emulated environment for Roton services to use.
        // - Load the world into that emulated environment.
        // - Unpack each board, one by one, exporting tiles and actor data.
        // This allows us to reuse all the existing load behavior.

        if (length < 2)
            throw new EndOfStreamException();

        var buffer = new byte[8192];
        if (stream.Read(buffer, 0, 2) < 2)
            throw new EndOfStreamException();

        var worldType = (Context)BinaryPrimitives.ReadInt16LittleEndian(buffer);
        using var mem = new MemoryStream(length);
        mem.Write(buffer, 0, 2);

        var remaining = length - 2;
        while (remaining > 0)
        {
            var amount = stream.Read(buffer, 0, buffer.Length);
            if (amount == 0)
                break;
            mem.Write(buffer, 0, amount);
            remaining -= amount;
        }

        if (remaining > 0)
            throw new EndOfStreamException();

        mem.Position = 0;

        var services = serviceProviderFactory(worldType);
        var ser = GetService<IWorldImporter>(services);
        ser.ImportWorld(mem);
        ConvertFromContext(services);
    }

    private void ConvertFromContext(IServiceProvider services)
    {
        var inWorld = GetService<IWorld>(services);
        var inBoard = GetService<IBoard>(services);
        var packedBoards = GetService<IBoardList>(services);
        var packer = GetService<IBoardPacker>(services);
        var tiles = GetService<ITiles>(services);
        var actors = GetService<IActorList>(services);

        World = new World
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

        foreach (var packedBoard in packedBoards)
        {
            byte[] reserved;

            try
            {
                var used = packer.Unpack(packedBoard.Data);
                reserved = [.. packedBoard.Data.AsSpan(used)];
            }
            catch
            {
                // If importing a board fails, at least preserve the packed board data
                // just in case the user wants to recover it somehow.
                World.Boards.Add(new Board
                {
                    IsCorrupted = true,
                    Reserved = packedBoard.Data
                });

                continue;
            }

            var outBoard = new Board
            {
                Width = tiles.Width,
                Height = tiles.Height,
                Tiles = new Tile[tiles.Width, tiles.Height],
                Name = packedBoard.Name,
                Camera = inBoard.Camera,
                Entrance = inBoard.Entrance,
                IsDark = inBoard.IsDark,
                MaxShots = inBoard.MaximumShots,
                RestartOnZap = inBoard.RestartOnZap,
                TimeLimit = TimeSpan.FromSeconds((ushort)inBoard.TimeLimit),
                Reserved = reserved
            };

            for (var y = 0; y < tiles.Height; y++)
            {
                for (var x = 0; x < tiles.Width; x++)
                    outBoard.Tiles[x, y] = tiles[new Location(x + 1, y + 1)];
            }

            foreach (var inActor in actors)
            {
                var outActor = new Actor
                {
                    Location = inActor.Location - 1,
                    Vector = inActor.Vector,
                    Cycle = inActor.Cycle,
                    Follower = inActor.Follower,
                    Leader = inActor.Leader,
                    Instruction = inActor.Instruction,
                    Length = inActor.Length,
                    P1 = inActor.P1,
                    P2 = inActor.P2,
                    P3 = inActor.P3,
                    Pointer = inActor.Pointer,
                    Script = new Script(inActor.Code.ToString()),
                    UnderTile = inActor.UnderTile,
                    Reserved = [.. MemoryMarshal.Cast<HWord, byte>(inActor.Reserved)]
                };

                outBoard.Actors.Add(outActor);
            }

            World.Boards.Add(outBoard);
        }
    }

    public void Reset(Context context)
    {
        var services = serviceProviderFactory(context);
        var manager = GetService<IWorldManager>(services);
        manager.ClearWorld();
        ConvertFromContext(services);
    }
}