using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IFeatures
{
    void RemoveItem(Location location);
    void EnterBoard();
    bool HandleTitleInput();
    void ShowInGameHelp();
    void HandlePlayerInput(IActor actor);
    bool CanPutTile(Location location);
    void ClearForest(Location location);
    string[] GetMessageLines();
    void ShowAbout();
    int BaseMemoryUsage { get; }
    void CleanUpOop(ref OopContext context);
    int GetColorMatchValue(int color);
    int GetAdjacent(Location location, int elementId);
}