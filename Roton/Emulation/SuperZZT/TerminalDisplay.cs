using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal class TerminalDisplay : Display
    {
        public TerminalDisplay(IDisplayInfo infoSource)
            : base(infoSource)
        {
        }

        public override void CreateStatusBar()
        {
            if (DisplayInfo.TitleScreen)
            {
            }
            else
            {
            }
        }
    }
}
