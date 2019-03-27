using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Core.Impl
{
    public abstract class GameSerializer : IGameSerializer
    {
        private readonly Lazy<IMemory> _memory;
        private readonly Lazy<IHeap> _heap;

        protected GameSerializer(Lazy<IMemory> memory, Lazy<IHeap> heap)
        {
            _memory = memory;
            _heap = heap;
        }

        private IMemory Memory
        {
            [DebuggerStepThrough] get => _memory.Value;
        }

        private IHeap Heap
        {
            [DebuggerStepThrough] get => _heap.Value;
        }

        public abstract int ActorCapacity { get; }

        public abstract int ActorDataCountOffset { get; }

        public abstract int ActorDataLength { get; }

        public abstract int ActorDataOffset { get; }

        public abstract int BoardDataLength { get; }

        public abstract int BoardDataOffset { get; }

        public abstract int BoardNameLength { get; }

        public abstract int BoardNameOffset { get; }

        public byte[] LoadBoardData(Stream source)
        {
            var reader = new BinaryReader(source);
            int length = reader.ReadInt16();
            return reader.ReadBytes(length);
        }

        public void LoadWorld(Stream source)
        {
            var reader = new BinaryReader(source);
            var header = reader.ReadBytes(WorldDataCapacity - 4);
            Memory.Write(WorldDataOffset, header, 0, WorldDataSize);
        }

        public byte[] PackBoard(ITiles tiles)
        {
            using (var mem = new MemoryStream())
            {
                var writer = new BinaryWriter(mem);
                writer.Write(Memory.Read(BoardNameOffset, BoardNameLength).ToArray());
                PackTiles(tiles, writer);
                writer.Write(Memory.Read(BoardDataOffset, BoardDataLength).ToArray());
                var actorCount = Memory.Read16(ActorDataCountOffset);
                writer.Write((short) actorCount);
                PackActors(writer, actorCount);
                writer.Flush();
                return mem.ToArray();
            }
        }

        public void SaveBoardData(Stream target, byte[] data)
        {
            var writer = new BinaryWriter(target);
            if (data.Length > short.MaxValue) // 32767 bytes max theoretical
            {
                throw Exceptions.DataTooLarge;
            }

            writer.Write((short) data.Length);
            writer.Write(data);
            writer.Flush();
        }

        public void SaveWorld(Stream target)
        {
            var worldBytes = new byte[WorldDataCapacity - 4];
            var worldData = Memory.Read(WorldDataOffset, WorldDataSize);
            worldData.CopyTo(worldBytes);
            target.Write(worldBytes, 0, worldBytes.Length);
        }

        public void UnpackBoard(ITiles tiles, byte[] data)
        {
            using (var mem = new MemoryStream(data))
            {
                var reader = new BinaryReader(mem);
                Memory.Write(BoardNameOffset, reader.ReadBytes(BoardNameLength)); // board name
                UnpackTiles(tiles, reader); // tiles
                Memory.Write(BoardDataOffset, reader.ReadBytes(BoardDataLength)); // board properties
                int actorCount = reader.ReadInt16();
                Memory.Write16(ActorDataCountOffset, actorCount); // actor count
                UnpackActors(reader, actorCount); // actors
            }
        }

        public abstract int WorldDataCapacity { get; }

        public abstract int WorldDataOffset { get; }

        public abstract int WorldDataSize { get; }

        private void PackActors(BinaryWriter target, int count)
        {
            // list of actors and saved code
            var savedCode = new Dictionary<int, int>();

            // backed up memory (so we don't modify the working version)
            var mem = new Memory();
            mem.Write(ActorDataOffset, Memory.Read(ActorDataOffset, ActorDataLength * (count + 1)));

            for (var i = 0; i <= count; i++)
            {
                var actor = new Actor(mem, Heap, ActorDataOffset + ActorDataLength * i);
                byte[] code = null;

                if (actor.Pointer != 0)
                {
                    code = Heap[actor.Pointer];

                    // check to see if the code needs to be stored
                    if (code != null)
                    {
                        if (savedCode.ContainsKey(actor.Pointer))
                        {
                            actor.Length = -savedCode[actor.Pointer];
                        }
                        else
                        {
                            savedCode[actor.Pointer] = i;
                        }
                    }

                    actor.Pointer = 0;
                }

                // write memory to stream
                target.Write(Memory.Read(actor.Offset, ActorDataLength).ToArray());

                // write code if applicable
                if (code != null)
                {
                    target.Write(code);
                }
            }
        }

        private static void PackTiles(ITiles tiles, BinaryWriter target)
        {
            var firstTile = tiles[new Location(1, 1)];
            var count = 0;
            var id = firstTile.Id;
            var color = firstTile.Color;

            for (var y = 1; y <= tiles.Height; y++)
            {
                for (var x = 1; x <= tiles.Width; x++)
                {
                    var tile = tiles[new Location(x, y)];
                    if (tile.Id != id || tile.Color != color || count == 255)
                    {
                        target.Write((byte) (count & 0xFF));
                        target.Write((byte) (id & 0xFF));
                        target.Write((byte) (color & 0xFF));
                        count = 0;
                        id = tile.Id;
                        color = tile.Color;
                    }

                    count++;
                }
            }

            if (count > 0)
            {
                target.Write((byte) (count & 0xFF));
                target.Write((byte) (id & 0xFF));
                target.Write((byte) (color & 0xFF));
            }
        }

        private void UnpackActors(BinaryReader source, int count)
        {
            // sanity check
            if (count > ActorCapacity)
            {
                throw Exceptions.CorruptedData;
            }

            // clean out code heap (there are no cross-board references)
            Heap.FreeAll();

            // load all actors
            var actorList = new List<IActor>();
            for (var i = 0; i <= count; i++)
            {
                var actor = new Actor(Memory, Heap, ActorDataOffset + ActorDataLength * i);
                Memory.Write(ActorDataOffset + ActorDataLength * i, source.ReadBytes(ActorDataLength));
                actor.Pointer = 0;
                if (actor.Length > 0)
                {
                    var code = source.ReadBytes(actor.Length);
                    var pointer = Heap.Allocate(code);
                    actor.Pointer = pointer;
                }

                actorList.Add(actor);
            }

            // now check to see if any are in #bind
            for (var i = 0; i <= count; i++)
            {
                if (actorList[i].Length < 0)
                {
                    var actorCodeSource = new Actor(Memory, Heap,
                        ActorDataOffset + -actorList[i].Length * ActorDataLength);
                    actorList[i].Length = actorCodeSource.Length;
                    actorList[i].Pointer = actorCodeSource.Pointer;
                }
            }
        }

        private static void UnpackTiles(ITiles tiles, BinaryReader source)
        {
            var count = 0;
            var id = 0;
            var color = 0;

            for (var y = 1; y <= tiles.Height; y++)
            {
                for (var x = 1; x <= tiles.Width; x++)
                {
                    if (count == 0)
                    {
                        count = source.ReadByte();
                        id = source.ReadByte();
                        color = source.ReadByte();
                    }

                    var tile = tiles[new Location(x, y)];
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
}