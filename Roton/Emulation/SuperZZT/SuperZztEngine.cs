using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class SuperZztEngine
    {
        protected override void StartMain()
        {
            StateData.GameSpeed = 4;
            StateData.DefaultSaveName = "SAVED";
            StateData.DefaultBoardName = "TEMP";
            StateData.DefaultWorldName = "MONSTER";
            Hud.GenerateFadeMatrix();
            if (!StateData.WorldLoaded)
            {
                ClearWorld();
            }
            SetGameMode();
            TitleScreenLoop();
        }
    }
}