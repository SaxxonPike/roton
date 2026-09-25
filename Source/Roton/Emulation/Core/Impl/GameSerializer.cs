using System.IO;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class GameSerializer(
    IMemory memory,
    IFacts facts)
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
        var header = reader.ReadBytes(facts.WorldDataCapacity - 4);
        memory.Write(facts.WorldDataOffset, header, 0, facts.WorldDataSize);
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
        var worldBytes = new byte[facts.WorldDataCapacity - 4];
        var worldData = memory.Read(facts.WorldDataOffset, facts.WorldDataSize);
        worldData.CopyTo(worldBytes);
        target.Write(worldBytes, 0, worldBytes.Length);
    }
}