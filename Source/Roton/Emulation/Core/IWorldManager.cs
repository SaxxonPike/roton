namespace Roton.Emulation.Core;

public interface IWorldManager
{
    bool LoadWorld(string name, bool savedGame);
    void SaveWorld(string name);
    void ClearWorld();
    void OpenWorld();
    bool RestoreWorld();

    /// <remarks>
    /// RoZ: BoardClose
    /// </remarks>
    void PackBoard();

    /// <remarks>
    /// RoZ: BoardOpen
    /// </remarks>
    void UnpackBoard(int index);

    void SetBoard(int boardIndex);
    void ClearBoard();
}