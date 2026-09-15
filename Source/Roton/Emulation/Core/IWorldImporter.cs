using System.IO;

namespace Roton.Emulation.Core;

public interface IWorldImporter
{
    bool ImportWorldData(int worldType, Stream stream);
    bool ExportWorldData(Stream stream);
}