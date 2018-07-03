using System.Collections.Generic;
using Roton.Core;

namespace Roton.Interface.Audio.Composition
{
    public interface IAudioComposer : ISpeaker
    {
        IEnumerable<int> ComposeAudio();
    }
}
