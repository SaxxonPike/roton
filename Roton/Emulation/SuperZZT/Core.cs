using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal partial class Core
    {
        internal override void StartMain()
        {
            GameSpeed = 4;
            DefaultSaveName = "SAVED";
            DefaultBoardName = "TEMP";
            DefaultWorldName = "TOWN";
            Display.GenerateFadeMatrix();
            if (!WorldLoaded)
            {
                ClearWorld();
            }
            SetGameMode();
            TitleScreenLoop();
        }
    }
}
