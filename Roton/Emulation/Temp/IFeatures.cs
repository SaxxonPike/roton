using Roton.Emulation.Data;

namespace Roton.Emulation.Temp
{
    public interface IFeatures
    {
        void LockActor(int index);
        void UnlockActor(int index);
        bool IsActorLocked(int index);
        void RemoveItem(IXyPair location);
        string GetWorldName(string baseName);
        void EnterBoard();
        bool HandleTitleInput(int hotkey);
        void ShowInGameHelp();
        void ExecuteMessage(IOopContext context);
        void Init();
        void HandlePlayerInput(IActor actor, int hotkey);
        bool CanPutTile(IXyPair location);
        void ClearForest(IXyPair location);
        void CleanUpPassageMovement();
    }
}