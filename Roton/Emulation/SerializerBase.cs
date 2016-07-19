using System;
using System.Collections.Generic;
using System.IO;
using Roton.Core;
using Roton.Internal;

namespace Roton.Emulation
{
    internal abstract class SerializerBase
    {
        public SerializerBase(Memory memory)
        {
            Memory = memory;
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

        public Memory Memory { get; }

        private void PackActors(BinaryWriter target, int count)
        {
            // list of actors and saved code
            var savedCode = new Dictionary<int, int>();
            var heap = Memory.CodeHeap;

            // backed up memory (so we don't modify the working version)
            var mem = new Memory();
            mem.Write(ActorDataOffset, Memory.Read(ActorDataOffset, ActorDataLength*(count + 1)));

            for (var i = 0; i <= count; i++)
            {
                var writeCode = false;
                var actor = new MemoryActor(mem, ActorDataOffset + ActorDataLength*i);
                var code = heap[actor.Pointer];

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
                        writeCode = true;
                    }
                }
                actor.Pointer = 0;

                // write memory to stream
                target.Write(Memory.Read(actor.Offset, ActorDataLength));

                // write code if applicable
                if (writeCode)
                {
                    target.Write(code.ToBytes());
                }
            }
        }

        public byte[] PackBoard(MemoryTileCollectionBase tiles)
        {
            using (var mem = new MemoryStream())
            {
                var writer = new BinaryWriter(mem);
                writer.Write(Memory.Read(BoardNameOffset, BoardNameLength));
                PackTiles(tiles, writer);
                writer.Write(Memory.Read(BoardDataOffset, BoardDataLength));
                var actorCount = Memory.Read16(ActorDataCountOffset);
                writer.Write((short) actorCount);
                PackActors(writer, actorCount);
                writer.Flush();
                return mem.ToArray();
            }
        }

        private void PackTiles(MemoryTileCollectionBase tiles, BinaryWriter target)
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
            Array.Copy(worldData, 0, worldBytes, 0, WorldDataSize);
            target.Write(worldBytes, 0, worldBytes.Length);
        }

        private void UnpackActors(BinaryReader source, int count)
        {
            // sanity check
            if (count > ActorCapacity)
            {
                throw Exceptions.CorruptedData;
            }

            // clean out code heap (there are no cross-board references)
            var heap = Memory.CodeHeap;
            heap.FreeAll();

            // load all actors
            var actorList = new List<IActor>();
            for (var i = 0; i <= count; i++)
            {
                var actor = new MemoryActor(Memory, ActorDataOffset + ActorDataLength*i);
                Memory.Write(ActorDataOffset + ActorDataLength*i, source.ReadBytes(ActorDataLength));
                actor.Pointer = 0;
                if (actor.Length > 0)
                {
                    var code = source.ReadBytes(actor.Length);
                    var pointer = heap.Allocate(code.ToChars());
                    actor.Pointer = pointer;
                }
                actorList.Add(actor);
            }

            // now check to see if any are in #bind
            for (var i = 0; i <= count; i++)
            {
                if (actorList[i].Length < 0)
                {
                    var actorCodeSource = new MemoryActor(Memory,
                        ActorDataOffset + -actorList[i].Length*ActorDataLength);
                    actorList[i].Length = actorCodeSource.Length;
                    actorList[i].Pointer = actorCodeSource.Pointer;
                }
            }
        }

        public void UnpackBoard(MemoryTileCollectionBase tiles, byte[] data)
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

        private void UnpackTiles(MemoryTileCollectionBase tiles, BinaryReader source)
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

        public abstract int WorldDataCapacity { get; }

        public abstract int WorldDataOffset { get; }

        public abstract int WorldDataSize { get; }
    }
}