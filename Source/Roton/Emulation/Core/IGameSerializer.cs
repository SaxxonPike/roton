using System.IO;

namespace Roton.Emulation.Core;

public interface IGameSerializer
{
    byte[] LoadBoardData(Stream source);
    void LoadWorldData(Stream source);
    void SaveBoardData(Stream target, byte[] data);
    void SaveWorldData(Stream target);
}