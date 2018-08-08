using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface IFeatures
    {
        void LockActor(int index);
        void UnlockActor(int index);
        bool IsActorLocked(int index);
        void RemoveItem(IXyPair location);
        string GetWorldName(string baseName);
        void EnterBoard();
        bool HandleTitleInput();
        void ShowInGameHelp();
        IScrollState ExecuteMessage(IOopContext context);
        void HandlePlayerInput(IActor actor);
        bool CanPutTile(IXyPair location);
        void ClearForest(IXyPair location);
        void CleanUpPassageMovement();
        void ForcePlayerColor(int index);
        string[] GetMessageLines();
        void ShowAbout();
        int BaseMemoryUsage { get; }
        void CleanUpPauseMovement();
    }
}