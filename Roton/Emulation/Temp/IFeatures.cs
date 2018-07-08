namespace Roton.Core
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
    }
}