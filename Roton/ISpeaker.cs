using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public interface ISpeaker
    {
        void PlayDrum(int drum);
        void PlayNote(int note);
        void Stop();
    }
}
