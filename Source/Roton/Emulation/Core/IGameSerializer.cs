using System.IO;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IGameSerializer
{
    byte[] LoadBoardData(Stream source);
    void LoadWorld(Stream source);
    byte[] PackBoard(ITiles tiles);
    void SaveBoardData(Stream target, byte[] data);
    void SaveWorld(Stream target);
    void UnpackBoard(ITiles tiles, byte[] data);
}