using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IFeatures
{
    void RemoveItem(Location location);
    void EnterBoard();
    bool HandleTitleInput();
    void ShowInGameHelp();
    IScrollState? ExecuteMessage(ref OopContext context);
    void HandlePlayerInput(IActor actor);
    bool CanPutTile(Location location);
    void ClearForest(Location location);
    void CleanUpPassageMovement();
    void ForcePlayerColor(int index);
    string[] GetMessageLines();
    void ShowAbout();
    int BaseMemoryUsage { get; }
    void CleanUpPauseMovement();
    void CleanUpOop(ref OopContext context);
    int GetColorMatchValue(int color);
    int GetAdjacent(Location location, int elementId);
}