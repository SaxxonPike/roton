using System.IO;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Core.Impl;

public abstract class GameSerializer(
    IMemory memory)
    : IGameSerializer
{
    public byte[] LoadBoardData(Stream source)
    {
        var reader = new BinaryReader(source);
        int length = reader.ReadInt16();
        return reader.ReadBytes(length);
    }

    public void LoadWorldData(Stream source)
    {
        var reader = new BinaryReader(source);
        var header = reader.ReadBytes(WorldDataCapacity - 4);
        memory.Write(WorldDataOffset, header, 0, WorldDataSize);
    }

    public void SaveBoardData(Stream target, byte[] data)
    {
        var writer = new BinaryWriter(target);
        if (data.Length > short.MaxValue) // 32767 bytes max theoretical
        {
            throw Exceptions.DataTooLarge;
        }

        writer.Write((short)data.Length);
        writer.Write(data);
        writer.Flush();
    }

    public void SaveWorldData(Stream target)
    {
        var worldBytes = new byte[WorldDataCapacity - 4];
        var worldData = memory.Read(WorldDataOffset, WorldDataSize);
        worldData.CopyTo(worldBytes);
        target.Write(worldBytes, 0, worldBytes.Length);
    }

    public abstract int WorldDataCapacity { get; }

    public abstract int WorldDataOffset { get; }

    public abstract int WorldDataSize { get; }
}