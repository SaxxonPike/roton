using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    sealed internal partial class Core
    {
        public override void Act_Monitor(int index)
        {
            if (KeyPressed != 0)
            {
                BreakGameLoop = true;
            }
        }
    }
}
