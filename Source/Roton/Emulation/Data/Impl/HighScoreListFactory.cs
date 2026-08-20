using System;
using System.IO;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class HighScoreListFactory(IEngineAccessor engine, IFacts facts) : IHighScoreListFactory
{
    private IEngine Engine => engine.Instance;
    private IFacts Facts => facts;
        
    public IHighScoreList Load()
    {
        var list = new HighScoreList(Facts.HighScoreNameCount);
            
        var file = Engine.Disk.GetFile(Engine.GetHighScoreName(Engine.World.Name));
        if (file == null || file.Length != Facts.HighScoreNameCount * (Facts.HighScoreNameLength + 3))
            return list;

        using var stream = new MemoryStream(file);
        using var reader = new BinaryReader(stream);
        for (var i = 0; i < Facts.HighScoreNameCount; i++)
        {
            var nameLength = reader.ReadByte();
            var name = reader.ReadBytes(Facts.HighScoreNameLength);
            var score = reader.ReadInt16();
            var hs = list[i++];
            hs.Name = name.Take(nameLength).ToArray().ToStringValue();
            hs.Score = score;
        }

        return list;
    }

    public void Save(IHighScoreList highScoreList)
    {
        if (string.IsNullOrEmpty(Engine.World.Name))
            return;

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        foreach (var hs in highScoreList)
        {
            var nameLength = unchecked((byte) (hs.Name?.Length ?? 0));
            var nameBuffer = new byte[Facts.HighScoreNameLength];
            hs.Name.ToBytes(nameBuffer.AsSpan(0, Math.Min(nameLength, nameBuffer.Length)));
            var score = unchecked((short) hs.Score);
            writer.Write(nameLength);
            writer.Write(nameBuffer);
            writer.Write(score);
        }

        writer.Flush();
        Engine.Disk.PutFile(Engine.GetHighScoreName(Engine.World.Name), stream.ToArray());
    }
}