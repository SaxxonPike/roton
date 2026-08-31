using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IFeatures
{
    void RemoveItem(Location location);
    void EnterBoard();
    bool HandleTitleInput();
    void HandlePlayerInput(IActor actor);
    bool CanPutTile(Location location);
    void ClearForest(Location location);
    string[] GetMessageLines();
    int BaseMemoryUsage { get; }
    void CleanUpOop(ref OopContext context);
    int GetAdjacent(Location location, int elementId);
}