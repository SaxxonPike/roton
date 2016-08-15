using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Interface.Audio
{
    public interface IToneComposer
    {
        IEnumerable<int> ComposeAudio(int numberOfSamples);
        void PlayDrum(int index);
        void PlayTone(int note);
    }
}
