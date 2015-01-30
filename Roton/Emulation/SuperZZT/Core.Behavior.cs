using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal partial class Core
    {
        public override void Act_Monitor(int index)
        {
            base.Act_Monitor(index);
            MoveActorOnRiver(index);
        }

        public override void Act_Player(int index)
        {
            base.Act_Player(index);
            MoveActorOnRiver(index);
        }
    }
}
