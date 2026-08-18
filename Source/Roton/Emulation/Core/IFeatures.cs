using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Core;

public interface IFeatures
{
    void LockActor(int index);
    void UnlockActor(int index);
    bool IsActorLocked(int index);
    void RemoveItem(Location location);
    string GetWorldName(string baseName);
    string GetHighScoreName(string baseName);
    void EnterBoard();
    bool HandleTitleInput();
    void ShowInGameHelp();
    IScrollState ExecuteMessage(ref OopContext context);
    void HandlePlayerInput(IActor actor);
    bool CanPutTile(Location location);
    void ClearForest(Location location);
    void CleanUpPassageMovement();
    void ForcePlayerColor(int index);
    string[] GetMessageLines();
    void ShowAbout();
    int BaseMemoryUsage { get; }
    void CleanUpPauseMovement();
    string OpenWorld();
    string RestoreWorld();
    void CleanUpOop(ref OopContext context);
    int GetColorMatchValue(int color);
    void NotifyActorSentLabel(int index);
    string GetSaveName(string baseName);
    int GetAdjacent(Location location, int elementId);
}