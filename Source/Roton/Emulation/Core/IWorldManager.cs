namespace Roton.Emulation.Core;

public interface IWorldManager
{
    /// <remarks>
    /// RoZ: WorldLoad
    /// </remarks>
    bool LoadWorld(string name, bool savedGame);

    /// <remarks>
    /// RoZ: WorldSave
    /// </remarks>
    void SaveWorld(string name);

    /// <remarks>
    /// RoZ: WorldCreate
    /// </remarks>
    void ClearWorld();

    /// <remarks>
    /// RoZ: WorldUnload
    /// </remarks>
    void OpenWorld();

    /// <remarks>
    /// RoZ: WorldLoad (titleOnly)
    /// </remarks>
    bool RestoreWorld();

    /// <remarks>
    /// RoZ: BoardClose
    /// </remarks>
    void PackBoard();

    /// <remarks>
    /// RoZ: BoardOpen
    /// </remarks>
    void UnpackBoard(int index);

    /// <remarks>
    /// RoZ: BoardChange
    /// </remarks>
    void SetBoard(int boardIndex);

    /// <remarks>
    /// RoZ: BoardCreate
    /// </remarks>
    void ClearBoard();
}