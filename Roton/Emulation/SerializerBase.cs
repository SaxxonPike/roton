using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal class SerializerBase
    {
        public SerializerBase(Memory memory)
        {
            this.Memory = memory;
        }

        abstract public int ActorCapacity { get; }

        abstract public int ActorDataCountOffset { get; }

        abstract public int ActorDataLength { get; }

        abstract public int ActorDataOffset { get; }

        abstract public int BoardDataLength { get; }

        abstract public int BoardDataOffset { get; }

        abstract public int BoardNameLength { get; }

        abstract public int BoardNameOffset { get; }

        public Heap Heap
        {
            get { return Memory.Heap; }
        }

        public byte[] LoadBoardData(Stream source)
        {
            BinaryReader reader = new BinaryReader(source);
            int length = reader.ReadInt16();
            return reader.ReadBytes(length);
        }

        public void LoadWorld(Stream source)
        {
            BinaryReader reader = new BinaryReader(source);
            byte[] header = reader.ReadBytes(WorldDataCapacity - 4);
            Memory.Write(WorldDataOffset, header, 0, WorldDataSize);
        }

        public Memory Memory
        {
            get;
            private set;
        }

        private void PackActors(BinaryWriter target, int count)
        {
            // list of actors and saved code
            Dictionary<int, int> savedCode = new Dictionary<int, int>();

            // backed up memory (so we don't modify the working version)
            Memory mem = new Memory();
            mem.Write(ActorDataOffset, Memory.Read(ActorDataOffset, ActorDataLength * (count + 1)));

            for (int i = 0; i <= count; i++)
            {
                MemoryActor actor = new MemoryActor(mem, ActorDataOffset + (ActorDataLength * i));

                // check to see if the code needs to be stored
                if (actor.Pointer != 0 && Heap.Contains(actor.Pointer))
                {
                    if (savedCode.ContainsKey(actor.Pointer))
                    {
                        actor.Length = -savedCode[actor.Pointer];
                        actor.Pointer = 0;
                    }
                    else
                    {
                        savedCode[actor.Pointer] = i;
                    }
                }
                else
                {
                    actor.Pointer = 0;
                }

                // write memory to stream
                target.Write(Memory.Read(actor.Offset, ActorDataLength));

                // write code if applicable
                if (actor.Pointer != 0)
                {
                    target.Write(Heap[actor.Pointer]);
                }
            }
        }

        public byte[] PackBoard(MemoryTileCollectionBase tiles)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(mem);
                writer.Write(Memory.Read(BoardNameOffset, BoardNameLength));
                PackTiles(tiles, writer);
                writer.Write(Memory.Read(BoardDataOffset, BoardDataLength));
                int actorCount = Memory.Read16(ActorDataCountOffset);
                writer.Write((Int16)actorCount);
                PackActors(writer, actorCount);
                writer.Flush();
                return mem.ToArray();
            }
        }

        private void PackTiles(MemoryTileCollectionBase tiles, BinaryWriter target)
        {
            Tile firstTile = tiles[new Location(1, 1)];
            int count = 0;
            int id = firstTile.Id;
            int color = firstTile.Color;

            for (int y = 1; y <= tiles.Height; y++)
            {
                for (int x = 1; x <= tiles.Width; x++)
                {
                    Tile tile = tiles[new Location(x, y)];
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

            if (count > 0)
            {
                target.Write((byte)(count & 0xFF));
                target.Write((byte)(id & 0xFF));
                target.Write((byte)(color & 0xFF));
            }
        }

        public void SaveBoardData(Stream target, byte[] data)
        {
            BinaryWriter writer = new BinaryWriter(target);
            if (data.Length > Int16.MaxValue) // 32767 bytes max theoretical
            {
                throw Exceptions.DataTooLarge;
            }
            writer.Write((Int16)data.Length);
            writer.Write(data);
            writer.Flush();
        }

        public void SaveWorld(Stream target)
        {
            byte[] worldBytes = new byte[WorldDataCapacity - 4];
            byte[] worldData = Memory.Read(WorldDataOffset, WorldDataSize);
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

            // load all actors
            List<Actor> actorList = new List<Actor>();
            for (int i = 0; i <= count; i++)
            {
                MemoryActor actor = new MemoryActor(Memory, ActorDataOffset + (ActorDataLength * i));
                Memory.Write(ActorDataOffset + (ActorDataLength * i), source.ReadBytes(ActorDataLength));
                actor.Pointer = 0;
                if (actor.Length > 0)
                {
                    // load actor code into the heap
                    byte[] code = source.ReadBytes(actor.Length);
                    actor.Pointer = Heap.Allocate(code);
                }
                actorList.Add(actor);
            }
            // now check to see if any are in #bind
            for (int i = 0; i <= count; i++)
            {
                if (actorList[i].Length < 0)
                {
                    MemoryActor actorCodeSource = new MemoryActor(Memory, ActorDataOffset + (-actorList[i].Length * ActorDataLength));
                    actorList[i].Length = actorCodeSource.Length;
                    actorList[i].Pointer = actorCodeSource.Pointer;
                }
            }
        }

        public void UnpackBoard(MemoryTileCollectionBase tiles, byte[] data)
        {
            using (MemoryStream mem = new MemoryStream(data))
            {
                BinaryReader reader = new BinaryReader(mem);
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
            int count = 0;
            int id = 0;
            int color = 0;

            for (int y = 1; y <= tiles.Height; y++)
            {
                for (int x = 1; x <= tiles.Width; x++)
                {
                    if (count == 0)
                    {
                        count = source.ReadByte();
                        id = source.ReadByte();
                        color = source.ReadByte();
                    }
                    Tile tile = tiles[new Location(x, y)];
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

        abstract public int WorldDataCapacity { get; }

        abstract public int WorldDataOffset { get; }

        abstract public int WorldDataSize { get; }
    }
}
