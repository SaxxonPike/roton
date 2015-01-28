using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal partial class CoreBase
    {
        virtual internal byte[] PlayMusic(string music)
        {
            return new byte[0];
        }

        virtual internal void PlaySound(int priority, byte[] sound)
        {
        }

        virtual internal void StopSound()
        {
        }
    }
}
