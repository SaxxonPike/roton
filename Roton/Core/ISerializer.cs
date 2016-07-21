using System.IO;

namespace Roton.Core
{
    public interface ISerializer
    {
        byte[] LoadBoardData(Stream source);
        void LoadWorld(Stream source);
        byte[] PackBoard(ITileGrid tiles);
        void SaveBoardData(Stream target, byte[] data);
        void SaveWorld(Stream target);
        void UnpackBoard(ITileGrid tiles, byte[] data);
        int WorldDataCapacity { get; }
    }
}
