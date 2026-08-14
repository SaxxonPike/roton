using System.IO;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IGameSerializer
{
    int ActorCapacity { get; }
    int ActorDataCountOffset { get; }
    int ActorDataLength { get; }
    int ActorDataOffset { get; }
    int BoardDataLength { get; }
    int BoardDataOffset { get; }
    int BoardNameLength { get; }
    int BoardNameOffset { get; }
    int WorldDataCapacity { get; }
    int WorldDataOffset { get; }
    int WorldDataSize { get; }
    byte[] LoadBoardData(Stream source);
    void LoadWorld(Stream source);
    byte[] PackBoard(ITiles tiles);
    void SaveBoardData(Stream target, byte[] data);
    void SaveWorld(Stream target);
    void UnpackBoard(ITiles tiles, byte[] data);
}