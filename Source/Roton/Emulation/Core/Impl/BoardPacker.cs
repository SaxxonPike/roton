using System;
using System.Collections.Generic;
using System.IO;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class BoardPacker(
    IMemory memory,
    ICodeHeap heap,
    ITiles tiles,
    IFacts facts,
    IFactory factory)
    : IBoardPacker
{
    private ITiles _tiles = tiles;

    public byte[] Pack()
    {
        using var mem = new MemoryStream();
        var writer = new BinaryWriter(mem);
        writer.Write([.. memory.Read(facts.BoardNameOffset, facts.BoardNameLength)]);
        PackTiles(writer);
        writer.Write([.. memory.Read(facts.BoardDataOffset, facts.BoardDataLength)]);
        short actorCount = memory.GetRef<Word>(facts.ActorDataCountOffset);
        writer.Write(actorCount);
        PackActors(writer, actorCount);
        writer.Flush();
        return mem.ToArray();
    }

    public int Unpack(byte[] data)
    {
        using var mem = new MemoryStream(data);
        var reader = new BinaryReader(mem);
        memory.Write(facts.BoardNameOffset, reader.ReadBytes(facts.BoardNameLength)); // board name
        UnpackTiles(reader); // tiles
        memory.Write(facts.BoardDataOffset, reader.ReadBytes(facts.BoardDataLength)); // board properties
        int actorCount = reader.ReadInt16();
        memory.Write16(facts.ActorDataCountOffset, actorCount); // actor count
        UnpackActors(reader, actorCount); // actors
        return (int)mem.Position;
    }

    private void PackActors(BinaryWriter target, int count)
    {
        // list of actors and saved code
        var savedCode = new Dictionary<int, int>();

        // backed up memory (so we don't modify the working version)
        var mem = new Memory();
        mem.Write(facts.ActorDataOffset, memory.Read(facts.ActorDataOffset, facts.ActorDataLength * (count + 1)));

        for (var i = 0; i <= count; i++)
        {
            var actor = factory.Create<Actor>(
                facts.ActorDataOffset + facts.ActorDataLength * i,
                facts.ActorDataLength
            );

            var code = Span<char>.Empty;

            if (actor.Pointer != 0)
            {
                code = heap[actor.Pointer];

                // check to see if the code needs to be stored
                if (!code.IsEmpty)
                {
                    if (savedCode.TryGetValue(actor.Pointer, out var value))
                    {
                        actor.Length = -value;
                    }
                    else
                    {
                        savedCode[actor.Pointer] = i;
                    }
                }

                actor.Pointer = 0;
            }

            // write memory to stream
            target.Write([.. memory.Read(actor.Offset, facts.ActorDataLength)]);

            // write code if applicable
            if (code.IsEmpty)
                continue;

            var codeBytes = new byte[code.Length];
            Cp437.CharsToBytes(code, codeBytes);
            target.Write(codeBytes);
        }
    }

    private void PackTiles(BinaryWriter target)
    {
        var firstTile = _tiles[new Location(1, 1)];
        var count = 0;
        var id = firstTile.Id;
        var color = firstTile.Color;

        for (var y = 1; y <= _tiles.Height; y++)
        {
            for (var x = 1; x <= _tiles.Width; x++)
            {
                var tile = _tiles[new Location(x, y)];
                if (tile.Id != id || tile.Color != color || count == 255)
                {
                    target.Write((byte)(count & 0xFF));
                    target.Write((byte)(id & 0xFF));
                    target.Write((byte)(color & 0xFF));
                    count = 0;
                    id = tile.Id;
                    color = tile.Color;
                }

                count++;
            }
        }

        if (count <= 0)
            return;

        target.Write((byte)(count & 0xFF));
        target.Write((byte)(id & 0xFF));
        target.Write((byte)(color & 0xFF));
    }

    private void UnpackActors(BinaryReader source, int count)
    {
        var buffer = (stackalloc char[short.MaxValue]);

        // sanity check
        if (count > facts.ActorCapacity)
        {
            throw Exceptions.CorruptedData;
        }

        // clean out code heap (there are no cross-board references)
        heap.FreeAll();

        // load all actors
        var actorList = new List<IActor>();
        for (var i = 0; i <= count; i++)
        {
            var actor = factory.Create<Actor>(
                facts.ActorDataOffset + facts.ActorDataLength * i,
                facts.ActorDataLength
            );

            memory.Write(facts.ActorDataOffset + facts.ActorDataLength * i, source.ReadBytes(facts.ActorDataLength));
            actor.Pointer = 0;
            if (actor.Length > 0)
            {
                var code = source.ReadBytes(actor.Length);
                Cp437.BytesToChars(code, buffer);
                var pointer = heap.Allocate(buffer.Slice(0, actor.Length));
                actor.Pointer = pointer;
            }

            actorList.Add(actor);
        }

        // now check to see if any are in #bind
        for (var i = 0; i <= count; i++)
        {
            if (actorList[i].Length >= 0)
                continue;

            var actorCodeSource = factory.Create<Actor>(
                facts.ActorDataOffset + -actorList[i].Length * facts.ActorDataLength,
                facts.ActorDataLength
            );

            actorList[i].Length = actorCodeSource.Length;
            actorList[i].Pointer = actorCodeSource.Pointer;
        }
    }

    private void UnpackTiles(BinaryReader source)
    {
        var count = 0;
        var id = 0;
        var color = 0;

        for (var y = 1; y <= _tiles.Height; y++)
        {
            for (var x = 1; x <= _tiles.Width; x++)
            {
                if (count == 0)
                {
                    count = source.ReadByte();
                    id = source.ReadByte();
                    color = source.ReadByte();
                }

                ref var tile = ref _tiles[new Location(x, y)];
                tile.Id = id;
                tile.Color = color;
                count = (count - 1) & 0xFF;
            }
        }

        // sanity check
        if (count > 0)
        {
            throw Exceptions.CorruptedData;
        }
    }
}