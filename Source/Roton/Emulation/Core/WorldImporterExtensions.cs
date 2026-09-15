using System.Buffers.Binary;
using System.IO;

namespace Roton.Emulation.Core;

public static class WorldImporterExtensions
{
    public static bool ImportWorld(this IWorldImporter importer, Stream stream)
    {
        var typeBytes = new byte[2];
        if (stream.Read(typeBytes, 0, 2) < 2)
            throw new EndOfStreamException();
        return importer.ImportWorldData(BinaryPrimitives.ReadInt16LittleEndian(typeBytes), stream);
    }

    public static bool ExportWorld(this IWorldImporter importer, int worldType, Stream stream)
    {
        var typeBytes = new byte[2];
        BinaryPrimitives.WriteInt16LittleEndian(typeBytes, (short)worldType);
        stream.Write(typeBytes, 0, 2);
        return importer.ExportWorldData(stream);
    }
}