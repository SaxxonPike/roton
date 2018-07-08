using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface IMisc
    {
        void EnterBoard();
        void ExecuteMessage(IOopContext context);
        void Init();
        void HandlePlayerInput(IActor actor, int hotkey);
        bool HandleTitleInput(int hotkey);
        void RemoveItem(IXyPair location);
        void ShowInGameHelp();
        string GetWorldName(string baseName);
    }
}