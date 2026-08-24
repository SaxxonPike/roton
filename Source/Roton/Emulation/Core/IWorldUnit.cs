namespace Roton.Emulation.Core;

public interface IWorldUnit
{
    bool LoadWorld(string name, bool savedGame);
    void SaveWorld(string name);
    void ClearWorld();
    void OpenWorld();
    bool RestoreWorld();
    void PackBoard();
    void UnpackBoard(int index);
    void SetBoard(int boardIndex);
    void ClearBoard();
}