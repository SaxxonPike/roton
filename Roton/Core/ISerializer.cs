using System.IO;

namespace Roton.Core
{
    public interface ISerializer
    {
        int ActorCapacity { get; }
        int ActorDataCountOffset { get; }
        int ActorDataLength { get; }
        int ActorDataOffset { get; }
        int BoardDataLength { get; }
        int BoardDataOffset { get; }
        int BoardNameLength { get; }
        int BoardNameOffset { get; }
        byte[] LoadBoardData(Stream source);
        void LoadWorld(Stream source);
        byte[] PackBoard(ITileGrid tiles);
        void SaveBoardData(Stream target, byte[] data);
        void SaveWorld(Stream target);
        void UnpackBoard(ITileGrid tiles, byte[] data);
        int WorldDataCapacity { get; }
        int WorldDataOffset { get; }
        int WorldDataSize { get; }
    }
}