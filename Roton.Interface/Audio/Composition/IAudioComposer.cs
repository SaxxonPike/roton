using System.Collections.Generic;
using Roton.Emulation.Core;

namespace Roton.Interface.Audio.Composition
{
    public interface IAudioComposer : ISpeaker
    {
        IEnumerable<int> ComposeAudio();
    }
}
