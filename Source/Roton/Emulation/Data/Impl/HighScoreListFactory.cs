using System;
using System.IO;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class HighScoreListFactory(
    IFacts facts,
    IFileSystem fileSystem,
    IWorld world)
    : IHighScoreListFactory
{
    public IHighScoreList Load()
    {
        var list = new HighScoreList(facts.HighScoreNameCount);
            
        var file = fileSystem.GetFile($"{world.Name}.{facts.HighScoreExtension}");
        if (file == null || file.Length != facts.HighScoreNameCount * (facts.HighScoreNameLength + 3))
            return list;

        using var stream = new MemoryStream(file);
        using var reader = new BinaryReader(stream);
        for (var i = 0; i < facts.HighScoreNameCount; i++)
        {
            var nameLength = reader.ReadByte();
            var name = reader.ReadBytes(facts.HighScoreNameLength);
            var score = reader.ReadInt16();
            var hs = list[i++];
            hs.Name = name.Take(nameLength).ToArray().ToStringValue();
            hs.Score = score;
        }

        return list;
    }

    public void Save(IHighScoreList highScoreList)
    {
        if (string.IsNullOrEmpty(world.Name))
            return;

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        foreach (var hs in highScoreList)
        {
            var nameLength = unchecked((byte) (hs.Name?.Length ?? 0));
            var nameBuffer = new byte[facts.HighScoreNameLength];
            hs.Name.ToBytes(nameBuffer.AsSpan(0, Math.Min(nameLength, nameBuffer.Length)));
            var score = unchecked((short) hs.Score);
            writer.Write(nameLength);
            writer.Write(nameBuffer);
            writer.Write(score);
        }

        writer.Flush();
        fileSystem.PutFile($"{world.Name}.{facts.HighScoreExtension}", stream.ToArray());
    }
}